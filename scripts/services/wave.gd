extends Node2D

@export var amplitude := 20.0   # Height of the wave
@export var speed := 2.0        # Wave speed
@export var spacing := 0.5      # Phase offset between children

var time := 0.0
var base_positions := []
var texture

func _ready():
	texture=ImageTexture.create_from_image(Image.load_from_file(Skins.get_resource_path("keysq_main.png")))
	base_positions.clear()
	for child in get_children():
		if child is Node2D:
			base_positions.append(child.position)

func _process(delta):
	time += delta * speed

	for i in range(get_child_count()):
		var child = get_child(i)
		if child is Node2D:
			if child.texture != texture:
				child.texture = texture
			child.position.y = base_positions[i].y + sin(time + i * spacing) * amplitude
