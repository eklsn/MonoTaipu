extends Node2D

var viewer_node
var max_combo : int = 0
var taipus : int = 0
var goods : int = 0
var greats : int = 0
var mehs : int = 0
var accuracy: float = 0
var score : int = 0
var ttlclick : int = -1
var combo_counter : int = 0
var time : int
var hp : float = 100
var misses : int = 0
var click_sound
var miss_sound
var clsound
func _ready() -> void:
	viewer_node = $Preview
	viewer_node.editor = false
	Globalaud.get_node("Music").stop()
	Global.apply_config(Global.config)
	$fade.anim_fade(-0.1)
	Globalaud.menu = false
	viewer_node.file = Global.KEYMAN_LEVEL
	Global.KEYMAN_LEVEL = "null"
	viewer_node.paused = true
	#if get_tree().get_current_scene().get_name() == "Editor":
	#	editor = true
	viewer_node.gamepath=Global.gamepath
	if viewer_node.file == "null":
		viewer_node.file = await Global.open_var_with_dialog(PackedStringArray(["*.taipu ; TAIPU levels"]))
		
	click_sound = AudioStreamMP3.load_from_file(Skins.get_resource_path("hit.mp3"))
	miss_sound = AudioStreamMP3.load_from_file(Skins.get_resource_path("miss.mp3"))
	clsound = AudioStreamPlayer.new()
	clsound.max_polyphony = 64
	clsound.volume_linear = 1
	clsound.bus = "SFX"
	clsound.stream = click_sound
	add_child(clsound)
	viewer_node.lvlstart()
func _process(delta: float) -> void:
	$Label2.text = "SCORE: "+str(int(round(score)))+"\nAvg accuracy: "+str(int(round(accuracy)))+"%"+"\nMISSED: "+str(misses)
	if !viewer_node.paused:
		$combo.score = combo_counter
		hp-=viewer_node.lvlmeta["minus_hp_idle"]*100*delta
		hp=clamp(hp,0,100)
		$hpbar.value = lerp($hpbar.value,float(hp),0.5*20*delta)
		if combo_counter>max_combo:
			max_combo=combo_counter
		accuracy=score/ttlclick
		if (!viewer_node.vidonly && viewer_node.time>Globalaud.get_node("Music").stream.get_length()-0.05) or (viewer_node.vidonly && get_node("VideoStreamPlayer").get_stream_length()-0.01<viewer_node.time) or (viewer_node.keys[viewer_node.keys.size()-1][0]+2<viewer_node.time && ((!viewer_node.vidonly && Globalaud.get_node("Music").stream.get_length()-time>60) or get_node("VideoStreamPlayer").get_stream_length()-time>60)):
			Globalaud.get_node("fader").fade(0.05,1,0)
			await get_tree().create_timer(1.0).timeout
			Global.RES_goods = goods
			Global.RES_taipus = taipus
			Global.RES_mehs = mehs
			Global.RES_greats = greats
			Global.RES_misses = misses
			Global.RES_score = score
			Global.RES_lvlmeta = viewer_node.lvlmeta
			Global.RES_acc = accuracy
			Global.RES_combo =  max_combo
			Global.RES_file = viewer_node.file
			get_tree().change_scene_to_file("res://scenes/results.tscn")
func _unhandled_input(event: InputEvent) -> void:
	print(event)
	if event is InputEventKey:
		if event.pressed:
			if !viewer_node.editor and not event.is_echo() and !viewer_node.paused:
				if viewer_node.keyorder.has(OS.get_keycode_string(event.keycode)) && !viewer_node.keyorder.get(OS.get_keycode_string(event.keycode)).is_empty():
					
					while !is_instance_valid(viewer_node.keyorder.get(OS.get_keycode_string(event.keycode))[0]) && viewer_node.keyorder.get(OS.get_keycode_string(event.keycode)).has(0):
						viewer_node.keyorder.get(OS.get_keycode_string(event.keycode)).pop_front()
					if is_instance_valid(viewer_node.keyorder.get(OS.get_keycode_string(event.keycode))[0]):
						var curkey
						curkey = 0
						curkey = viewer_node.keyorder.get(OS.get_keycode_string(event.keycode))[0]
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
								curkey.hit(viewer_node.time,rank)
								hp+=accuracy*5
								viewer_node.keyorder.get(OS.get_keycode_string(event.keycode)).pop_at(0)
								clsound.play()
			if !viewer_node.editor && !viewer_node.paused:
				if event.keycode == KEY_F12 and !event.shift_pressed:
					reset()
				if event.keycode == KEY_F12 and event.shift_pressed:
					get_tree().change_scene_to_file("res://scenes/game.tscn")
			if event.keycode == KEY_ESCAPE and event.shift_pressed:
				quit()

func miss(idx):
	print(hp)
	if hp==0:
		viewer_node.paused=true
		$gameover.visible=true
		Globalaud.get_node("Music").stream_paused = true
		get_node("VideoStreamPlayer").paused = true
	else:
		if ttlclick == -1:
			ttlclick=1
		else:
			ttlclick+=1
		combo_counter = 0
		$combo.score = 0
		misses+=1
		hp-=viewer_node.lvlmeta["minus_hp_on_miss"]

func _on_try_pressed() -> void:
	viewer_node.paused=false
	reset()
	$gameover.visible = false


func _on_quit_pressed() -> void:
	quit()

func quit():
	viewer_node.paused=true
	$fade.anim_fade(0.05)
	Globalaud.get_node("Backout").play()
	await get_tree().create_timer(0.05).timeout
	Globalaud.menu = true
	if !Globalaud.get_node("Music").playing:
		Globalaud.get_node("Music").stream_paused = false
	get_node("VideoStreamPlayer").paused = false
	Globalaud.get_node("Music").loop = true
	Globalaud.get_node("vidplace").stream = get_node("VideoStreamPlayer").stream
	Globalaud.get_node("vidplace").stream_position = get_node("VideoStreamPlayer").stream_position
	Globalaud.get_node("vidplace").play()
	Globalaud.get_node("vidplace").volume_db = 0
	
	Globalaud.get_node("fader").fade(0.3,0,1)
	get_tree().change_scene_to_file("res://scenes/levelselect.tscn")

func reset():
	score = 0
	misses = 0
	time = 0
	viewer_node.i=0
	viewer_node.time=0
	ttlclick = -1
	combo_counter=0
	print("reset")
	#keyindex=[]
	viewer_node.killkeys()
	Globalaud.get_node("Music").stream_paused = false
	Globalaud.get_node("Music").seek(time)
	get_node("VideoStreamPlayer").paused = false
	hp=100
	if get_node("VideoStreamPlayer").visible:
		get_node("VideoStreamPlayer").stream_position = time
	hp=100
	
