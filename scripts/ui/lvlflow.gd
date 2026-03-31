extends Node3D

var level_files : Array
var offset : Vector3 = Vector3(0.1,0,0)
var selection : int = 0
var tile_nodes : Array
var camera
var new_campos : Vector3
var enabled : bool = false
func init(levels):
	get_tree().current_scene.get_node("fade").anim_fade(-0.05)
	level_files=levels
	camera = get_node("camera")
	new_campos = camera.position
	for i in range(level_files.size()):
		var temptile
		temptile=load("res://prefabs/menutile.tscn").instantiate()
		temptile.position=offset*i
		temptile.myindex=i
		if Global.open_json(level_files[i])[0].has("img_cover"):
			temptile.texture=ImageTexture.create_from_image(Image.load_from_file(level_files[i].get_base_dir()+"/"+Global.open_json(level_files[i])[0].get("img_cover")))
			temptile.custom_texture = true
			temptile.imgrefresh=true
		temptile.pixel_size= 5.1 / temptile.texture.get_size().y
		add_child(temptile)
	if Globalaud.menu == true && Globalaud.get_node("Music").playing && Globalaud.current_track != "":
		selection = level_files.find(Globalaud.current_track)
	Globalaud.menu = true
	enabled=true

func _process(delta: float) -> void:
	new_campos.x = offset.x*(selection)
	camera.position = lerp(camera.position,new_campos,5*delta)
	if enabled:
		if Global.open_json(level_files[selection])[0].has("name"):
			get_tree().current_scene.get_node("track").text = Global.open_json(level_files[selection])[0].get("name")
		else:
			get_tree().current_scene.get_node("track").text = "Unknown Track"
		if Global.open_json(level_files[selection])[0].has("lvl_author"):
			get_tree().current_scene.get_node("mapper").text = "Map by "+Global.open_json(level_files[selection])[0].get("lvl_author")
		else:
			get_tree().current_scene.get_node("mapper").text = "Unknown Mapper"
		if Global.open_json(level_files[selection])[0].has("song_author"):
			get_tree().current_scene.get_node("author").text = Global.open_json(level_files[selection])[0].get("song_author")
		else:
			get_tree().current_scene.get_node("author").text = "Unknown Author"
		if (Input.is_action_just_pressed("ui_right") or Input.is_action_just_pressed("mwheel_down")) && selection<level_files.size()-1:
			selection+=1
			$right.play()
		if (Input.is_action_just_pressed("ui_left") or Input.is_action_just_pressed("mwheel_up")) && selection>0:
			selection-=1
			$left.play()
		if (Input.is_key_pressed(KEY_ENTER)):
			enabled=false
			Globalaud.get_node("Levelstart").play()
			Globalaud.get_node("fader").fade(-0.005,db_to_linear(Globalaud.get_node("Music").volume_db),0)
			get_tree().current_scene.get_node("fade").anim_fade(0.1)
			Global.KEYMAN_LEVEL = level_files[selection]
			await get_tree().create_timer(0.3).timeout
			if Global.picker_mode == "play":
				get_tree().change_scene_to_file("res://scenes/game.tscn")
			else:
				get_tree().change_scene_to_file("res://scenes/editor.tscn")
			
		if (Input.is_key_pressed(KEY_ESCAPE)):
			get_tree().current_scene.get_node("fade").anim_fade(0.1)
			enabled=false
			await get_tree().create_timer(0.1).timeout
			get_tree().change_scene_to_file("res://scenes/mainmenu.tscn")
	
