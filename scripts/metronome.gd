extends Node2D


@export var beat : int = 0
@export var max : int = 4
@export var s_click : AudioStream
@export var click : AudioStream
@export var bpm : float = 120
var timer : float = 0
@export var enabled : bool = false
func _process(delta: float) -> void:
	if enabled:
		timer+=delta
		if timer>=60.0/bpm:
			if beat >= max:
				beat = 1
			if beat == 0:
				$s_click.play()
			else:
				$click.play()
			beat+=1
			timer=0
