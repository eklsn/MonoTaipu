extends Node
# GameWindow script by EKLSN.

var mode: String = "scale"
var scale
var manual_size: Vector2 = Vector2(640,480)
var current_size : Vector2
var target_size : Vector2
var fullscreen := false

func _ready():
	current_size = get_window().size
	match mode:
		"scale":
			scale = floor(min((DisplayServer.screen_get_size().x-1)/get_viewport().size.x, (DisplayServer.screen_get_size().y-1)/get_viewport().size.y))
			if scale < 1: scale = 1
			target_size = current_size*scale
		"manual_size":
			target_size = manual_size

	var screen_size: Vector2i = DisplayServer.screen_get_size(DisplayServer.window_get_current_screen())
	var usable_area = screen_size * 0.75
	if target_size.x > usable_area.x or target_size.y > usable_area.y:
		var shrink_factor = min(usable_area.x / target_size.x, usable_area.y / target_size.y)
		target_size *= shrink_factor

	change_size(target_size)

func change_size(size):
	get_window().size = size
	var screen_size : Vector2i = DisplayServer.screen_get_size(DisplayServer.window_get_current_screen())
	var centered = DisplayServer.screen_get_position() + Vector2i(
		screen_size.x / 2 - get_window().size.x / 2,
		screen_size.y / 2 - get_window().size.y / 2
	)
	get_window().position = centered

func toggle_fullscreen():
	fullscreen = !fullscreen
	get_window().mode = Window.MODE_FULLSCREEN if fullscreen else Window.MODE_WINDOWED
	if !fullscreen:
		change_size(target_size)

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventKey and event.pressed and !event.echo:
		if event.keycode == KEY_F11 or (event.keycode == KEY_ENTER and event.alt_pressed):
			toggle_fullscreen()

func _process(delta: float) -> void:
	current_size = get_window().size
