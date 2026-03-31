extends Node2D

var rank

func _ready() -> void:
	var file = FileAccess.open(Global.gamepath + "/skins/skin.txt", FileAccess.READ)
	if file != null:
		var skin_name = file.get_as_text().strip_edges()
		print(skin_name)
		file.close()
		var texture_path = Global.gamepath + "/skins/" + skin_name + "/"
		$snd.stream = AudioStreamMP3.load_from_file(texture_path+"results.mp3")
		$snd.play()
	$try.visible = false
	$main.visible = false
	if (Global.RES_misses+Global.RES_greats+Global.RES_goods+Global.RES_mehs == 0) && (Global.RES_combo==Global.RES_taipus):
		rank="SS"
	elif Global.RES_misses == 0 && Global.RES_acc>90:
		rank="S"
	elif (Global.RES_acc>90) or (Global.RES_acc>80 and Global.RES_misses == 0):
		rank="A"
	elif Global.RES_acc>80:
		rank="B"
	elif Global.RES_acc>60:
		rank="C"
	else:
		rank="D"
	$map.text = Global.RES_lvlmeta["name"]+" by "+Global.RES_lvlmeta["lvl_author"]
	$rank.text = rank
	$results/score.text = "SCORE: "+str(int(Global.RES_score))
	$results/misses.text = "MISSES: "+str(Global.RES_misses)
	$results/accuracy.text = "ACCURACY: "+str(int(Global.RES_acc))+"%"
	$results/greats.text = "GREAT's: "+str(Global.RES_greats)
	$results/goods.text = "GOOD's: "+str(Global.RES_goods)
	$results/mehs.text = "MEH's: "+str(Global.RES_mehs)
	$results/taipus.text = "TAIPU's: "+str(Global.RES_taipus)
	$results/maxcombo.text = "MAX COMBO: "+str(Global.RES_combo)+"x"
	$goodjob.start()
	await $goodjob.fin
	$results/score.start()
	await $results/score.fin
	$results/accuracy.start()
	await $results/accuracy.fin
	$results/maxcombo.start()
	await $results/maxcombo.fin
	$results/taipus.start()
	await $results/taipus.fin
	$results/greats.start()
	await $results/greats.fin
	$results/goods.start()
	await $results/goods.fin
	$results/mehs.start()
	await $results/mehs.fin
	$results/misses.start()
	await $results/misses.fin
	$try.visible = true
	$main.visible = true
	$beta.start()
