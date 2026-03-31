extends Node


func get_resource_path(resname):
	var file = FileAccess.open(Global.gamepath + "/skins/skin.txt", FileAccess.READ)
	if file != null:
		var skin_name = file.get_as_text().strip_edges()
		file.close()
		return(Global.gamepath + "/skins/" + skin_name + "/" + resname)
