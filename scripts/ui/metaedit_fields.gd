extends LineEdit

@export var meta_name : String
@export var export_data : String
@export var accepts : String = "String"
var oldtext = ""
var fdata
func _process(delta: float) -> void:
	if oldtext!=text:
		fdata=text
		match accepts:
			"int":
				if not text.is_valid_int():
					text=oldtext
				fdata=int(text)
			"float":
				if not text.is_valid_float():
					text=oldtext
				fdata=float(text)
		oldtext=text
		get_tree().current_scene.viewer_node.lvlmeta[meta_name] = fdata
		print(get_tree().current_scene.viewer_node.lvlmeta[meta_name])
	
