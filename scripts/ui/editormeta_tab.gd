extends Node2D

func init(lvlmeta,filter:String = "all"):
	for i in get_children():
		if "meta_name" in i:
			if filter == "all":
				if lvlmeta.has(i.meta_name) && str(lvlmeta[i.meta_name]) != "":
					i.text=str(lvlmeta[i.meta_name])
			else:
				if lvlmeta.has(i.meta_name) && str(lvlmeta[i.meta_name])  != "" && i.meta_name == filter:
					i.text=str(lvlmeta[i.meta_name])
