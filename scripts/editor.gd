extends Node2D

var viewer_node
var time
var scroll_factor : float = 1.0
@export var fast_devscroll = 4.0
@export var normal_devscroll = 2.0
func _ready() -> void:
	viewer_node = $Preview
	viewer_node.editor = true
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
		viewer_node.file = await Global.open_var_with_dialog(PackedStringArray(["*.taipu ; TAIPU level"]))
	print(viewer_node.file)
	await viewer_node.lvlstart()
	$MetaTab.init(viewer_node.lvlmeta)
func _unhandled_input(event):
	if event is InputEventMouseButton and $Preview.visible and !$hotkeys.visible:
		if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
			viewer_node.time += 0.05 * viewer_node.scroll_factor
			Globalaud.get_node("Music").seek(viewer_node.time)
			if get_node("VideoStreamPlayer").visible:
				get_node("VideoStreamPlayer").stream_position = viewer_node.time
		elif event.button_index == MOUSE_BUTTON_WHEEL_UP:
			if (viewer_node.time - 0.05 * viewer_node.scroll_factor) >= 0:
				viewer_node.time -= 0.05 * viewer_node.scroll_factor
				Globalaud.get_node("Music").seek(viewer_node.time)
				if get_node("VideoStreamPlayer").visible:
					get_node("VideoStreamPlayer").stream_position = viewer_node.time
	if event is InputEventKey:
		if event.pressed and $Preview.visible:
			if event.keycode == KEY_RIGHT:
				time += 0.05 * scroll_factor
				Globalaud.get_node("Music").seek(time)
			if event.keycode == KEY_F1:
				viewer_node.speed_scale = 1
			if event.keycode == KEY_F2:
				viewer_node.speed_scale = 0.5
			if event.keycode == KEY_F3:
				viewer_node.speed_scale = 0.25
			if event.keycode == KEY_F4:
				viewer_node.speed_scale = 2
			if event.keycode == KEY_F9:
				get_node("VideoStreamPlayer").visible = not get_node("VideoStreamPlayer").visible
			if event.keycode == KEY_F12 and event.shift_pressed:
				get_tree().change_scene_to_file("res://scenes/editor.tscn")
			if event.keycode == KEY_DELETE and event.shift_pressed:
				viewer_node.i=0
				viewer_node.keyoriglen = 0
				viewer_node.keys.clear()
				viewer_node.editkeys.clear()
				viewer_node.killkeys()
			if event.keycode == KEY_HOME:
				time = 0
				Globalaud.get_node("Music").seek(time)
				if get_node("VideoStreamPlayer").visible:
					get_node("VideoStreamPlayer").stream_position = time
			if event.keycode == KEY_F10:
				var savekeys = 0
				savekeys = 0
				savekeys = viewer_node.editkeys
				savekeys = savekeys.filter(func(x): return x != [0])
				savekeys.sort_custom(func (a, b): return a[0] < b[0])
				savekeys.insert(0,viewer_node.lvlmeta)
				savekeys[0].set("exportbuild",Global.build)
				savekeys[0].set("offset",viewer_node.lvlmeta["pre_ring_time"]+viewer_node.lvlmeta["ring_time"])
				savekeys = JSON.stringify(savekeys,"")
				Global.save_var_with_dialog(savekeys)
			elif event.keycode == KEY_LEFT:
				if (time - 0.05 * scroll_factor) >= 0:
					time -= 0.05 * scroll_factor
					Globalaud.get_node("Music").seek(time)
			elif event.keycode == KEY_SPACE:
				if viewer_node.paused:
					viewer_node.paused = false
					Globalaud.get_node("Music").stream_paused = false
					Globalaud.get_node("Music").seek(viewer_node.time)
					get_node("VideoStreamPlayer").paused = false
					if get_node("VideoStreamPlayer").visible:
						get_node("VideoStreamPlayer").stream_position = viewer_node.time
				else:
					viewer_node.paused = true
					Globalaud.get_node("Music").stream_paused = true
					get_node("VideoStreamPlayer").paused = true
			else:
				if len(OS.get_keycode_string(event.keycode)) == 1:
					var arr_temp : Array = [0,"A","0,0"]
					arr_temp[viewer_node.datapoints["timing"]] = snappedf(viewer_node.time,0.001)
					arr_temp[viewer_node.datapoints["key"]] = OS.get_keycode_string(event.keycode)
					arr_temp[viewer_node.datapoints["position"]] = "0,0"
					viewer_node.editkeys.append(arr_temp)
					viewer_node.create_key(OS.get_keycode_string(event.keycode), "0,0", viewer_node.time-viewer_node.song_offset, viewer_node.keys.size()-1)
			if event.keycode == KEY_ESCAPE and event.shift_pressed:
				quit()
func _process(delta: float) -> void:
	var pstxt
	pstxt = ""
	if viewer_node.paused:
		pstxt="PAUSED\n"
	$Preview/curtime.text = pstxt+"%02d:%05.2f" % [floor(viewer_node.time/60),snappedf(fmod(viewer_node.time,60),0.01)]
func delkey(idx):
	if viewer_node.visible:
		viewer_node.keyindex[idx][1].queue_free()
		viewer_node.editkeys[viewer_node.keyindex[idx][0]] = [0]

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
	get_tree().change_scene_to_file("res://scenes/mainmenu.tscn")
	
func import_resource(meta,path):
	var tempdir = DirAccess.open(viewer_node.file.get_base_dir())
	print(tempdir.copy_absolute(path,viewer_node.file.get_base_dir()+"/"+path.get_file()))
	viewer_node.lvlmeta[meta]=path.get_file()
	print(viewer_node.lvlmeta)
	match(meta):
		"audiofile":
			viewer_node.res_reload("audio")
		"videofile":
			viewer_node.res_reload("video")
		"imagefile":
			viewer_node.res_reload("image")
	$MetaTab.init(viewer_node.lvlmeta, meta)
