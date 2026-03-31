extends Node2D
var level_files
var lvlmeta
var vidonly
var startseek = 0
var music
var current_track
var menu : bool = false

func play_randomleveltrack():
	level_files = Global.scanned_levels
	level_files = Global.get_files_with_extensions(Global.gamepath+"/maps",[".taipu"])
	var newtrack = level_files[randi_range(0,level_files.size()-1)]
	if newtrack != current_track:
		play_leveltrack(level_files[randi_range(0,level_files.size()-1)])
	else:
		play_randomleveltrack()

func play_leveltrack(file):
	current_track = file
	lvlmeta = Global.open_json(file)[0]
	var muspath = file.get_base_dir()+"/"+lvlmeta["audiofile"]
	if lvlmeta.has("preview_timestamp"):
		startseek=lvlmeta["preview_timestamp"]
	if muspath.right(3).to_lower() == "mp3":
		music = AudioStreamMP3.load_from_file(muspath)
	elif muspath.right(3).to_lower() == "ogg":
		music = AudioStreamOggVorbis.load_from_file(muspath)
	elif muspath.right(3).to_lower() == "wav" or muspath.right(4).to_lower() == "wave":
		music = AudioStreamWAV.load_from_file(muspath)
	else:
		music = load(muspath)
	Globalaud.get_node("Music").set_stream(music)
	if Globalaud.get_node("Music").stream == null:
			Globalaud.get_node("vidplace").stream = load(file.get_base_dir()+"/"+lvlmeta["videofile"])
			Globalaud.get_node("vidplace").stream_position = startseek
			Globalaud.get_node("vidplace").play()
			vidonly = true
	else:
		Globalaud.get_node("Music").seek(startseek)
		Globalaud.get_node("Music").play()
		Globalaud.get_node("Music").seek(startseek)


func _on_music_finished() -> void:
	print(menu)
	if menu:
		play_randomleveltrack()
		Globalaud.get_node("fader").fade(0.25,0,1)
