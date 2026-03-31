extends Node


var curaud : Dictionary

func play(tag : String, obj : Node, stream : AudioStream):
	curaud[tag] = AudioStreamPlayer.new()
	obj.add_child(curaud.get(tag))
	curaud.get(tag).stream = stream
	curaud.get(tag).play()
	await curaud.get(tag).finished
	if is_instance_valid(curaud[tag]):
		print(tag)
		curaud.get(tag).queue_free()
	curaud.erase(tag)

func GET(tag):
	return curaud.get(tag)

func pause(tag : String):
	curaud.get(tag).stream_paused = true

func unpause(tag):
	curaud.get(tag).stream_paused = false
	
func volset(tag : String, vol : float):
	curaud.get(tag).volume_db = linear_to_db(vol)

func stop(tag : String):
	curaud.get(tag).playing = false
	curaud.get(tag).queue_free()
	curaud.erase(tag)
