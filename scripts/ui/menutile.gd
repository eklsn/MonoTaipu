extends Sprite3D

var myindex : int = 0
var newpos : Vector3
var imgrefresh : bool = false
var default_texture : ImageTexture
var custom_texture : bool = false
func _ready() -> void:
	default_texture = ImageTexture.create_from_image(Image.load_from_file(Skins.get_resource_path("ico_lvldefault.png")))
	if !custom_texture:
		texture = default_texture
	newpos=position

func _process(delta: float) -> void:
	if imgrefresh:
		material_override.set_shader_parameter("texture_albedo", texture)

	if get_parent().name == "lvlflow":
		rotation.y=clamp(5*(get_parent().get_node("camera").position.x-position.x),-0.75,0.75)
		if get_parent().selection==myindex:
			$highlight.visible=true
			material_override.set_shader_parameter("dim", 1)
			newpos.z=-0.02
		else:
			$highlight.visible=false
			material_override.set_shader_parameter("dim", 0.75)
			newpos.z=-0.15
		position=lerp(position,newpos,15*delta)
	
