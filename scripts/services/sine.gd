extends Node2D

@export var amplitude := 20.0
@export var speed := 2.0

var time := 0.0
var start_y := 0.0

func _ready():
	start_y = position.y

func _process(delta):
	time += delta * speed
	position.y = start_y + sin(time) * amplitude
