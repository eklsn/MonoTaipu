extends Node2D
var level_files
var file
var lvlmeta
var vidonly
var startseek = 0
func _ready() -> void:
	Globalaud.menu = true
	$fade.anim_fade(-0.1)
func _process(delta: float) -> void:
	if !Globalaud.get_node("Music").playing and !Globalaud.get_node("vidplace").is_playing():
		Globalaud.play_randomleveltrack()
		Globalaud.get_node("fader").fade(0.25,0,1)
	
	
