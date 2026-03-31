extends Node

var file = "null"
var keys : Array = []
var editkeys : Array = []
var keyoriglen : int = 0
var lvlmeta : Dictionary
var keyorder : Dictionary
var keyindex : Array
var time : float = 0
var offtime : float = 0
var type = 0
var i = 0
var prevtime : float = 0
var keybrdmode = true
var song_offset : float = 0.7
var gamepath = ""
var difficulty = 1
var music
var hp : float = 100.0
var score = 0
var ttlclick = -1
var misses : int = 0
var combo_counter = 0
var vidonly = false
var clsound
var max_combo : int = 0
var taipus : int = 0
var goods : int = 0
var greats : int = 0
var mehs : int = 0
var accuracy: float = 0
@export var speed_scale : float = 1.0

@onready var keynode = preload("res://prefabs/key.tscn")


var click_sound
var miss_sound

@export var editor : bool = true
var paused : bool = false
var scroll_factor : float = 1.0
@export var fast_devscroll = 4.0
@export var normal_devscroll = 2.0
var datapoints = {
	"timing" = 0,
	"key" = 1,
	"position" = 2
}

func _ready() -> void:
	Globalaud.get_node("Music").stop()
	Global.apply_config(Global.config)
	$fade.anim_fade(-0.1)
	Globalaud.menu = false
	file = Global.KEYMAN_LEVEL
	Global.KEYMAN_LEVEL = "null"
	click_sound = AudioStreamMP3.load_from_file(Skins.get_resource_path("hit.mp3"))
	miss_sound = AudioStreamMP3.load_from_file(Skins.get_resource_path("miss.mp3"))
	clsound = AudioStreamPlayer.new()
	clsound.max_polyphony = 64
	clsound.volume_linear = 1
	clsound.bus = "SFX"
	clsound.stream = click_sound
	add_child(clsound)
	paused = true
	#if get_tree().get_current_scene().get_name() == "Editor":
	#	editor = true
	gamepath=Global.gamepath
	if file == "null":
		file = await open_var_with_dialog()
	lvlstart()
func lvlstart():
	var json_string = FileAccess.get_file_as_string(file)
	var json = JSON.new()
	if json.parse(json_string) == OK:
		keys = json.data
		lvlmeta = keys[0]
		keys.pop_front()
		editkeys = keys
		keyoriglen = keys.size()
		json=null
		type = lvlmeta["readscheme"]
		var muspath = file.get_base_dir()+"/"+lvlmeta["audiofile"]
		if muspath.right(3).to_lower() == "mp3":
			music = AudioStreamMP3.load_from_file(muspath)
		elif muspath.right(3).to_lower() == "ogg":
			music = AudioStreamOggVorbis.load_from_file(muspath)
		elif muspath.right(3).to_lower() == "wav" or muspath.right(4).to_lower() == "wave":
			music = AudioStreamWAV.load_from_file(muspath)
		else:
			music = load(muspath)
		if !lvlmeta.has("exportbuild"):
			lvlmeta["exportbuild"]="b02"
		if int(lvlmeta["exportbuild"].right(len(lvlmeta["exportbuild"])-1)) < 7:
			if !(lvlmeta.has("hit_timeframe") && lvlmeta.has("ring_time") && lvlmeta.has("pre_ring_time")):
				lvlmeta["hit_timeframe"] = 0.2
				lvlmeta["ring_time"] = 0.5
				lvlmeta["pre_ring_time"] = 0.2
			if lvlmeta.has("offset"):
				lvlmeta.erase("offset")
		if !lvlmeta.has("minus_hp_on_miss"):
			lvlmeta["minus_hp_on_miss"]=25
		if !lvlmeta.has("minus_hp_idle"):
			lvlmeta["minus_hp_idle"]=0.05
		Globalaud.get_node("fader").fading=false
		Globalaud.get_node("Music").set_stream(music)
		Globalaud.current_track = file
		Globalaud.get_node("Music").volume_linear = 1
		Globalaud.get_node("Music").loop = false
		$VideoStreamPlayer.stream = load(file.get_base_dir()+"/"+lvlmeta["videofile"])
		Globalaud.get_node("Music").seek(time)
		$VideoStreamPlayer.stream_position = time
		song_offset = lvlmeta["pre_ring_time"]+lvlmeta["ring_time"]
		if lvlmeta.has("imagefile"):
			$TextureRect.texture = ImageTexture.create_from_image(Image.load_from_file(file.get_base_dir()+"/"+lvlmeta["imagefile"]))
		if Globalaud.get_node("Music").stream == null:
			$VideoStreamPlayer.volume_db = 0
			vidonly = true
		await get_tree().create_timer(1).timeout
		if keybrdmode:
			$keybrd.visible = true
		Globalaud.get_node("Music").play()
		if vidonly:
			$VideoStreamPlayer.volume_db = 0
			print()
		$VideoStreamPlayer.play()
		paused = false
	else:
		push_error("JSON parse error: " + json.get_error_message())
		if editor:
			#var newf_dial = NativeConfirmationDialog.new()
			#newf_dial.dialog_text = "The file you've selected is invalid or empty.\nWould you like to create a new TAIPU level?"
			#newf_dial.buttons_texts = 1
			#newf_dial.title = "New level?"
			#newf_dial.dialog_icon = 2
			#add_child(newf_dial)
			#newf_dial.show()
			#await newf_dial.confirmed
			#newf_dial = NativeAcceptDialog.new()
			#newf_dial.dialog_text = "After clicking OK specify a file for the new level.\nIt would be best, if the level and the data will be in a seperate folder."
			#newf_dial.title = "TAIPU"
			#newf_dial.dialog_icon = 0
			#add_child(newf_dial)
			#newf_dial.show()
			#await newf_dial.confirmed
			lvlmeta = {
				"name" : "Level name here",
				"song_author" : "Song author name here",
				"lvl_author" : "Level author name here",
				"audiofile" : "Put the .mp3 .ogg or .wav file in the same folder, and put the filename here.",
				"videofile" : "Put the .ogv file in the same folder, and put the filename here.",
				"imagefile" : "Put the .png or .jpg file in the same folder, and put the filename here.",
				"pre_ring_time" : 0.2,
				"ring_time" : 0.5,
				"hit_timeframe" : 0.2,
				"readscheme" : 0,
				"minus_hp_on_miss" : 25,
				"minus_hp_idle" : 0.5,
				"exportbuild" : Global.build,
				"znote" : "Prompt the video, audio or image files, and open the level in Taipu."
			}
			editkeys = []
			var savekeys = editkeys
			savekeys.insert(0,lvlmeta)
			savekeys = JSON.stringify(savekeys,"\t")
			await save_var_with_dialog(savekeys)
			#newf_dial = NativeAcceptDialog.new()
			#newf_dial.dialog_text = "Open the level you've just saved\nin any text editing software, and follow\nthe instructions."
			#newf_dial.title = "TAIPU"
			#newf_dial.dialog_icon = 0
			#add_child(newf_dial)
			#newf_dial.show()
		else:
			pass
			#var newf_dial = NativeAcceptDialog.new()
			#newf_dial.dialog_text = "The file you've selected is not a TAIPU level."
			#newf_dial.title = "TAIPU"
			#newf_dial.dialog_icon = 2
			#add_child(newf_dial)
			#newf_dial.show()

func _process(delta: float) -> void:
	if !editor && !paused:
		if combo_counter>max_combo:
			max_combo=combo_counter
		accuracy=score/ttlclick
		if (!vidonly && time>Globalaud.get_node("Music").stream.get_length()-0.05) or (vidonly && $VideoStreamPlayer.get_stream_length()-0.01<time):
			Global.RES_goods = goods
			Global.RES_taipus = taipus
			Global.RES_mehs = mehs
			Global.RES_greats = greats
			Global.RES_misses = misses
			Global.RES_score = score
			Global.RES_lvlmeta = lvlmeta
			Global.RES_acc = accuracy
			Global.RES_combo =  max_combo
			Global.RES_file = file
			get_tree().change_scene_to_file("res://scenes/results.tscn")
	if !editor && !paused:
		hp-=lvlmeta["minus_hp_idle"]*100*delta
		hp=clamp(hp,0,100)
		$hpbar.value = lerp($hpbar.value,float(hp),0.5*20*delta)
	if (Globalaud.get_node("Music").get_playback_position()-time > 0.01 or Globalaud.get_node("Music").get_playback_position()-time < -0.01) and (!paused and !vidonly):
		time = Globalaud.get_node("Music").get_playback_position()
	if !$VideoStreamPlayer.is_playing() && !paused && $VideoStreamPlayer.get_stream_length()-0.01>time:
		$VideoStreamPlayer.paused = false
		$VideoStreamPlayer.stream_position = time
		$VideoStreamPlayer.play()
	if !Globalaud.get_node("Music").playing && !paused && !Globalaud.get_node("Music").stream == null:
		if Globalaud.get_node("Music").stream.get_length()-0.01>time:
			Globalaud.get_node("Music").stream_paused = false
			Globalaud.get_node("Music").seek(time)
			Globalaud.get_node("Music").play()
	if !editor:
		$Label2.text = "SCORE: "+str(int(round(score)))+"\nAvg accuracy: "+str(int(round(accuracy)))+"%"+"\nMISSED: "+str(misses)
		$combo.score = combo_counter
	Globalaud.get_node("Music").pitch_scale = speed_scale
	$VideoStreamPlayer.speed_scale = speed_scale
	if editor:
		if ($VideoStreamPlayer.stream_position-time > 0.15 or $VideoStreamPlayer.stream_position-time < -0.15) && $VideoStreamPlayer.visible:
			$VideoStreamPlayer.stream_position = time
			
	if !paused:
		time+=delta*speed_scale
	offtime=time+song_offset
	if Input.is_key_pressed(KEY_SHIFT):
		scroll_factor = 2.0
	elif Input.is_key_pressed(KEY_CTRL):
		scroll_factor = 0.25
	else:
		scroll_factor = 1.0
	if editor:
		var pstxt
		pstxt = ""
		if paused:
			pstxt="PAUSED\n"
		$Label.text = pstxt+"Time: "+str(snappedf(time,0.001))
	while i < keyoriglen and(
		type == 0 and keys[i][datapoints["timing"]] <= offtime or
		type == 1 and prevtime + keys[i][datapoints["timing"]] <= offtime):
		prevtime = offtime
		create_key(keys[i][datapoints["key"]], keys[i][datapoints["position"]], keys[i][datapoints["timing"]]-song_offset, i)
		i+=1

func create_key(key, pos, timerec, arrpos):
	var ourkey
	
	ourkey = keynode.instantiate()
	ourkey.key = key
	ourkey.hit_timeframe = lvlmeta["hit_timeframe"]
	ourkey.pre_ring_time = lvlmeta["pre_ring_time"]
	ourkey.ring_time = lvlmeta["ring_time"]
	ourkey.tstamp = timerec
	ourkey.position = str_to_var("Vector2("+pos+")")
	var shift : int = 0
	if keyorder.get(key) == null or keyorder.get(key).is_empty():
		var arr : Array = [null]
		keyorder.set(key,arr)
	
	while keyorder[key].get(shift) != null:
		keyorder.get(key).resize(shift+2)
		shift+=1
	keyindex.append([arrpos,ourkey,timerec])
	keyindex[len(keyindex)-1][0] = len(keyindex)-1
	ourkey.myindex = keyindex[len(keyindex)-1][0]
	keyorder[key].set(shift,ourkey)
	if keybrdmode:
		$keybrd.add_child(keyorder[key].get(shift))
	if ourkey == keyindex[ourkey.myindex][1]:
		pass
func _unhandled_input(event):
	if event is InputEventKey:
		if event.pressed:
			if !editor and not event.is_echo() and !paused:
				if keyorder.has(OS.get_keycode_string(event.keycode)) && !keyorder.get(OS.get_keycode_string(event.keycode)).is_empty():
					
					while !is_instance_valid(keyorder.get(OS.get_keycode_string(event.keycode))[0]) && keyorder.get(OS.get_keycode_string(event.keycode)).has(0):
						keyorder.get(OS.get_keycode_string(event.keycode)).pop_front()
					if is_instance_valid(keyorder.get(OS.get_keycode_string(event.keycode))[0]):
						var curkey
						curkey = 0
						curkey = keyorder.get(OS.get_keycode_string(event.keycode))[0]
						if !curkey.missed:
							var accuracy
							accuracy = pow((curkey.keytime-curkey.pre_ring_time) / curkey.ring_time, 2.0 * curkey.ring_time)
							if accuracy > 1:
								accuracy = 1 - pow(((curkey.keytime-curkey.pre_ring_time) - curkey.ring_time) / curkey.hit_timeframe, 2.0 / sqrt(curkey.hit_timeframe))
							if accuracy < 0:
								accuracy = 0
							var rank
							if accuracy > 0.9:
								rank = "TAIPU!!!"
								accuracy=1
								taipus+=1
							elif accuracy > 0.8:
								rank = "Great!"
								greats+=1
							elif accuracy > 0.75:
								rank = "Good!"
								goods+=1
							elif accuracy > 0.25:
								rank = "Meh."
								mehs+=1
							elif accuracy < 0.3:
								curkey.miss()
							accuracy=snapped(accuracy,0.1)
							if accuracy>0.3:
								score+=accuracy*100
								if ttlclick == -1:
									ttlclick=1
								else:
									ttlclick+=1
								combo_counter+=1
								curkey.hit(time,rank)
								hp+=accuracy*5
								keyorder.get(OS.get_keycode_string(event.keycode)).pop_at(0)
								clsound.play()
			if !editor && !paused:
				if event.keycode == KEY_F12 and !event.shift_pressed:
					reset()
				if event.keycode == KEY_F12 and event.shift_pressed:
					get_tree().change_scene_to_file("res://scenes/game.tscn")
			if event.keycode == KEY_ESCAPE and event.shift_pressed:
				quit()
						
						
func reset():
	score = 0
	misses = 0
	time = 0
	i=0
	ttlclick = -1
	combo_counter=0
	#keyindex=[]
	killkeys()
	Globalaud.get_node("Music").stream_paused = false
	Globalaud.get_node("Music").seek(time)
	$VideoStreamPlayer.paused = false
	hp=100
	if $VideoStreamPlayer.visible:
		$VideoStreamPlayer.stream_position = time
	hp=100
func killkeys():
	keyorder.clear()
	while keyindex.size() != 0:
		if is_instance_valid(keyindex[0][1]):
			keyindex[0][1].queue_free()
		keyindex.pop_at(0)
func delkey(idx):
	keyindex[idx][1].queue_free()
	editkeys[keyindex[idx][0]] = [0]

func miss(idx):
	if hp==0:
		paused=true
		$gameover.visible=true
		Globalaud.get_node("Music").stream_paused = true
		$VideoStreamPlayer.paused = true
	else:
		if ttlclick == -1:
			ttlclick=1
		else:
			ttlclick+=1
		combo_counter = 0
		$combo.score = 0
		misses+=1
		hp-=lvlmeta["minus_hp_on_miss"]

func save_var_with_dialog(value):
	var dialog := FileDialog.new()
	dialog.file_mode = FileDialog.FILE_MODE_SAVE_FILE
	dialog.access = FileDialog.ACCESS_FILESYSTEM
	dialog.use_native_dialog = true
	add_child(dialog)

	dialog.file_selected.connect(func(path):
		var f := FileAccess.open(path, FileAccess.WRITE)
		f.store_string(str(value))
		f.close()
	)

	dialog.popup_centered()
	await dialog.file_selected
	return(true)
	
func open_var_with_dialog() -> String:
	var dialog := FileDialog.new()
	dialog.file_mode = FileDialog.FILE_MODE_OPEN_FILE
	dialog.access = FileDialog.ACCESS_FILESYSTEM
	dialog.use_native_dialog = true
	add_child(dialog)
	
	dialog.popup_centered()

	var path = await dialog.file_selected
	return path


func quit():
	paused=true
	$fade.anim_fade(0.05)
	Globalaud.get_node("Backout").play()
	await get_tree().create_timer(0.05).timeout
	Globalaud.menu = true
	if !Globalaud.get_node("Music").playing:
		Globalaud.get_node("Music").stream_paused = false
	$VideoStreamPlayer.paused = false
	Globalaud.get_node("Music").loop = true
	Globalaud.get_node("vidplace").stream = $VideoStreamPlayer.stream
	Globalaud.get_node("vidplace").stream_position = $VideoStreamPlayer.stream_position
	Globalaud.get_node("vidplace").play()
	Globalaud.get_node("vidplace").volume_db = 0
	
	Globalaud.get_node("fader").fade(0.3,0,1)
	
	if editor:
		get_tree().change_scene_to_file("res://scenes/mainmenu.tscn")
	else:
		get_tree().change_scene_to_file("res://scenes/levelselect.tscn")

func _on_file_dialog_file_selected(path: String) -> void:
	file = path
	pass


func _on_try_pressed() -> void:
	paused=false
	reset()
	$gameover.visible = false


func _on_quit_pressed() -> void:
	quit()
