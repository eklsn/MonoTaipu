extends Node

var build = "GD10"
var buildnum = "260206"
var gamepath
var editor

var RES_score = 100
var RES_acc = 100
var RES_misses = 0
var RES_lvlmeta = {"name": "Bad Apple",
"lvl_author": "EKLSN"}
var RES_lvlfile = ""
var RES_triedagain : bool = false
var RES_taipus = 100
var RES_greats = 0
var RES_goods = 0
var RES_mehs = 0
var RES_combo = 100
var RES_file = "null"
var KEYMAN_LEVEL = "null"
var picker_mode = "play"
var scanned_levels
var delta = 0
var master_volume = 1
var music_volume = 1
var sfx_volume = 1
var config = {
	"master_volume" : 1,
	"music_volume" : 1,
	"sfx_volume" : 1,
}
# Called when the node enters the scene tree for the first time.



func _ready():
	# Determine base path
	if OS.has_feature("editor"):
		gamepath = "C:/Taipu"
	else:
		gamepath = OS.get_executable_path().get_base_dir()
	#var dir = DirAccess.open(gamepath)
	#if dir && !dir.dir_exists("savedata"):
	#	dir.make_dir("savedata")
	#if !dir.file_exists("savedata/config.json"):
	#	save_json(config,gamepath+"/savedata/config.json")
	#else:
	#	config = open_json(gamepath+"/savedata/config.json")
	#	apply_config(config)
	level_scan()
	apply_config(config)


func apply_config(conf):
	master_volume = config["master_volume"]
	music_volume = master_volume*config["music_volume"]
	sfx_volume = master_volume*config["sfx_volume"]
	AudioServer.set_bus_volume_linear(AudioServer.get_bus_index("Music"), music_volume)
	AudioServer.set_bus_volume_linear(AudioServer.get_bus_index("SFX"), sfx_volume)
	print("config applied")

func save_json(data, path):
	var jstring = JSON.stringify(data)
	var file = FileAccess.open(path, FileAccess.WRITE)
	if file:
		file.store_string(jstring)
		file.close()
	else:
		push_error("Failed to open file for writing")


func level_scan():
	scanned_levels = get_files_with_extensions(Global.gamepath+"/maps",[".taipu"])
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	Global.delta = delta
	pass

func dprint(node: Node, text: String) -> void:
	print("["+Time.get_time_string_from_system()+"] ["+node.get_script().get_path()+"("+node.get_name()+")]: "+text)

func get_files_with_extensions(path: String, extensions: Array[String]) -> Array[String]:
	var result: Array[String] = []
	var dir := DirAccess.open(path)
	if dir == null:
		push_error("Cannot open directory: %s" % path)
		return result
	dir.list_dir_begin()
	var file_name := dir.get_next()
	while file_name != "":
		if dir.current_is_dir():
			if file_name != "." and file_name != "..":
				result += get_files_with_extensions(path + "/" + file_name, extensions)
		else:
			for ext in extensions:
				if file_name.to_lower().ends_with(ext.to_lower()):
					result.append(path + "/" + file_name)
					break
		file_name = dir.get_next()
	dir.list_dir_end()
	return result

func open_json(file_path: String):
	if not FileAccess.file_exists(file_path):
		push_error("File does not exist: %s" % file_path)
		return {}
	
	var file = FileAccess.open(file_path, FileAccess.READ)
	if file == null:
		push_error("Failed to open file: %s" % file_path)
		return {}
	
	var content = file.get_as_text()
	file.close()
	
	var result = JSON.parse_string(content)
	
	return result

func save_var_with_dialog(value, filters: Variant = null) -> String:
	var dialog := FileDialog.new()
	dialog.file_mode = FileDialog.FILE_MODE_SAVE_FILE
	dialog.access = FileDialog.ACCESS_FILESYSTEM
	dialog.use_native_dialog = true

	if filters == null:
		dialog.filters = PackedStringArray(["*.* ; All Files"])
	else:
		dialog.filters = filters

	add_child(dialog)
	dialog.popup_centered()

	# Wait for either signal
	var path: String = ""
	var which = await dialog.file_selected

	# If we got here, file_selected fired
	path = which

	# Save
	var f := FileAccess.open(path, FileAccess.WRITE)
	if f:
		f.store_string(str(value))
		f.close()

	dialog.queue_free()
	return path




func open_var_with_dialog(filters: Variant = null) -> String:
	var dialog := FileDialog.new()
	dialog.file_mode = FileDialog.FILE_MODE_OPEN_FILE
	dialog.access = FileDialog.ACCESS_FILESYSTEM
	dialog.use_native_dialog = true

	if filters == null:
		dialog.filters = PackedStringArray(["*.* ; All Files"])
	else:
		dialog.filters = filters

	add_child(dialog)

	dialog.popup_centered()

	var path = await dialog.file_selected
	return path
