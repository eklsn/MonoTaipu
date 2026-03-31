extends Node2D

var move_pos: Vector2
var move_speed: float
var move_type := "lerp"

var scale_target: Vector2
var scale_speed: float
var scale_type := "lerp"

var fadedir: float
var effects := []

signal finished

func _process(delta: float) -> void:
	if "fade" in effects:
		modulate.a = clampf(modulate.a + fadedir, 0.0, 1.0)
		if modulate.a == 0.0 or modulate.a == 1.0:
			effects.erase("fade")

	if "move" in effects:
		position = position.move_toward(move_pos, move_speed*5*delta) if move_type == "square" else position.lerp(move_pos, move_speed*5*delta)
		if position.distance_to(move_pos) < 0.1:
			position = move_pos
			effects.erase("move")

	if "scale" in effects:
		scale = Vector2(
			move_toward(scale.x, scale_target.x, scale_speed*5*delta),
			move_toward(scale.y, scale_target.y, scale_speed*5*delta)
		) if scale_type == "square" else scale.lerp(scale_target, scale_speed*5*delta)
		if scale.distance_to(scale_target) < 0.001:
			scale = scale_target
			effects.erase("scale")


func move(pos: Vector2, type: String = "lerp", speed: float = 0.1) -> void:
	move_pos = pos
	move_type = type
	move_speed = speed
	if "move" not in effects:
		effects.append("move")


func do_scale(target: Vector2, type: String = "lerp", speed: float = 0.1) -> void:
	scale_target = target
	scale_type = type
	scale_speed = speed
	if "scale" not in effects:
		effects.append("scale")


func anim_fade(dir: float = -0.005) -> void:
	fadedir = dir
	modulate.a = 1.0 if dir < 0.0 else 0.0
	visible = true
	if "fade" not in effects:
		effects.append("fade")
