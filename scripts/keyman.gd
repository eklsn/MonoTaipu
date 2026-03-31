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
var stream_length = -1
var score = 0
var ttlclick = -1
var misses : int = 0
var combo_counter = 0
var vidonly = false
@export var speed_scale : float = 1.0

@onready var keynode = preload("res://prefabs/key.tscn")


@export var editor : bool = true
var paused : bool = false
var scroll_factor : float = 1.0

var datapoints = {
	"timing" = 0,
	"key" = 1,
	"position" = 2
}

func _ready() -> void:
	pass
func lvlstart():
	var json_string = FileAccess.get_file_as_string(file)
	var json = JSON.new()
	if json.parse(json_string) == OK:
		keys = json.data
		lvlmeta = keys[0]
		keys.pop_front()
		editkeys = keys.duplicate_deep(true)
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
		get_parent().get_node("VideoStreamPlayer").stream = load(file.get_base_dir()+"/"+lvlmeta["videofile"])
		Globalaud.get_node("Music").seek(time)
		get_parent().get_node("VideoStreamPlayer").stream_position = time
		song_offset = lvlmeta["pre_ring_time"]+lvlmeta["ring_time"]
		if lvlmeta.has("imagefile"):
			get_parent().get_node("TextureRect").texture = ImageTexture.create_from_image(Image.load_from_file(file.get_base_dir()+"/"+lvlmeta["imagefile"]))
		if Globalaud.get_node("Music").stream == null:
			get_parent().get_node("VideoStreamPlayer").volume_db = 0
			vidonly = true
		if !editor:
			await get_tree().create_timer(1).timeout
		if keybrdmode:
			$keybrd.visible = true
		Globalaud.get_node("Music").play()
		if vidonly:
			get_parent().get_node("VideoStreamPlayer").volume_db = 0
			print()
		get_parent().get_node("VideoStreamPlayer").play()
		paused = false
		if !vidonly:
			stream_length = Globalaud.get_node("Music").stream.get_length()
		else:
			stream_length = get_parent().get_node("VideoStreamPlayer").get_stream_length()
		return true
	else:
		push_error("JSON parse error: " + json.get_error_message())
		return false

func res_reload(resource):
	if resource == "audio":
		var muspath = file.get_base_dir()+"/"+lvlmeta["audiofile"]
		if muspath.right(3).to_lower() == "mp3":
			music = AudioStreamMP3.load_from_file(muspath)
		elif muspath.right(3).to_lower() == "ogg":
			music = AudioStreamOggVorbis.load_from_file(muspath)
		elif muspath.right(3).to_lower() == "wav" or muspath.right(4).to_lower() == "wave":
			music = AudioStreamWAV.load_from_file(muspath)
		else:
			music = load(muspath)
		Globalaud.get_node("fader").fading=false
		Globalaud.get_node("Music").set_stream(music)
		Globalaud.current_track = file
		Globalaud.get_node("Music").volume_linear = 1
		Globalaud.get_node("Music").loop = false
		Globalaud.get_node("Music").seek(time)
		if Globalaud.get_node("Music").stream == null:
			vidonly = true
		Globalaud.get_node("Music").play()
		if vidonly:
			get_parent().get_node("VideoStreamPlayer").volume_db = 0
		get_parent().get_node("VideoStreamPlayer").play()
		paused = false
		if !vidonly:
			stream_length = Globalaud.get_node("Music").stream.get_length()
	if resource == "video":
		get_parent().get_node("VideoStreamPlayer").stream = load(file.get_base_dir()+"/"+lvlmeta["videofile"])
		get_parent().get_node("VideoStreamPlayer").stream_position = time
		if Globalaud.get_node("Music").stream == null:
			get_parent().get_node("VideoStreamPlayer").volume_db = 0
			vidonly = true
		if keybrdmode:
			$keybrd.visible = true
		if vidonly:
			get_parent().get_node("VideoStreamPlayer").volume_db = 0
			print()
		get_parent().get_node("VideoStreamPlayer").play()
		paused = false
		if !vidonly:
			stream_length = Globalaud.get_node("Music").stream.get_length()
		else:
			stream_length = get_parent().get_node("VideoStreamPlayer").get_stream_length()
	if resource == "image":
		if lvlmeta.has("imagefile"):
			get_parent().get_node("TextureRect").texture = ImageTexture.create_from_image(Image.load_from_file(file.get_base_dir()+"/"+lvlmeta["imagefile"]))

func _process(delta: float) -> void:
	if (Globalaud.get_node("Music").get_playback_position()-time > 0.01 or Globalaud.get_node("Music").get_playback_position()-time < -0.01) and (!paused and !vidonly):
		time = Globalaud.get_node("Music").get_playback_position()
	if !get_parent().get_node("VideoStreamPlayer").is_playing() && !paused && get_parent().get_node("VideoStreamPlayer").get_stream_length()-0.01>time:
		get_parent().get_node("VideoStreamPlayer").paused = false
		get_parent().get_node("VideoStreamPlayer").play()
	if !Globalaud.get_node("Music").playing && !paused && !Globalaud.get_node("Music").stream == null:
		if Globalaud.get_node("Music").stream.get_length()-0.01>time:
			Globalaud.get_node("Music").stream_paused = false
			Globalaud.get_node("Music").seek(time)
			Globalaud.get_node("Music").play()
	Globalaud.get_node("Music").pitch_scale = speed_scale
	get_parent().get_node("VideoStreamPlayer").speed_scale = speed_scale
	if editor:
		if (get_parent().get_node("VideoStreamPlayer").stream_position-time > 0.15 or get_parent().get_node("VideoStreamPlayer").stream_position-time < -0.15) && get_parent().get_node("VideoStreamPlayer").visible:
			get_parent().get_node("VideoStreamPlayer").stream_position = time
			print("DESYNC!")
			
	if !paused:
		time+=delta*speed_scale
	offtime=time+song_offset
	if Input.is_key_pressed(KEY_SHIFT):
		scroll_factor = 2.0
	elif Input.is_key_pressed(KEY_CTRL):
		scroll_factor = 0.25
	else:
		scroll_factor = 1.0
		
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
						
						
func killkeys():
	keyorder.clear()
	while keyindex.size() != 0:
		if is_instance_valid(keyindex[0][1]):
			keyindex[0][1].queue_free()
		keyindex.pop_at(0)

func _on_file_dialog_file_selected(path: String) -> void:
	file = path
	pass
