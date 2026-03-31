extends Node2D

func _ready() -> void:
	$mastervol.value = Global.config["master_volume"]
	$musvol.value = Global.config["music_volume"]
	$sndvol.value = Global.config["sfx_volume"]

func _process(delta: float) -> void:
	Global.config["master_volume"] = $mastervol.value
	Global.config["music_volume"] = $musvol.value
	Global.config["sfx_volume"] = $sndvol.value
	Global.apply_config(Global.config)
