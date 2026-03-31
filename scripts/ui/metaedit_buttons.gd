extends Button

@export var meta_name : String
@export var export_data : String
@export var filters = PackedStringArray(["*.* ; All Files"])
var myfile : String
func _ready() -> void:
	self.connect("pressed",import)
	
func import():
	myfile = await Global.open_var_with_dialog(filters)
	print(myfile)
	get_tree().current_scene.import_resource(meta_name,myfile)
