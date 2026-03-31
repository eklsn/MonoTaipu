extends Node

var fading = false
var fade_value = 0.0
var target = 1.0

func fade(fadeval, start, target_val):
	fading = true
	fade_value = fadeval
	target = target_val
	# Start both sources at initial linear value
	get_parent().get_node("Music").volume_db = linear_to_db(start)
	get_parent().get_node("vidplace").volume_db = linear_to_db(start)

func _physics_process(delta):
	if fading:
		var music = get_parent().get_node("Music")
		var vid = get_parent().get_node("vidplace")
		var music_lin = db_to_linear(music.volume_db)
		var vid_lin = db_to_linear(vid.volume_db)
		music_lin += fade_value * delta
		vid_lin += fade_value * delta
		music_lin = clamp(music_lin, 0, target)
		vid_lin = clamp(vid_lin, 0, target)
		music.volume_db = linear_to_db(music_lin)
		vid.volume_db = linear_to_db(vid_lin)
		if abs(music_lin - target) < 0.01 and abs(vid_lin - target) < 0.01:
			fading = false
			print("stopped")
