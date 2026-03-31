extends HSlider
var is_dragging : bool =false
func _process(delta: float) -> void:
	if get_tree().current_scene.viewer_node.stream_length != -1 && !is_dragging:
		max_value = get_tree().current_scene.viewer_node.stream_length
		value = get_tree().current_scene.viewer_node.time
	if is_dragging:
		get_tree().current_scene.viewer_node.time = value
		Globalaud.get_node("Music").seek(value)

func _on_value_changed(value: float) -> void:
	pass
	#get_tree().current_scene.viewer_node.time = value
	#Globalaud.get_node("Music").seek(value)


func _on_drag_started() -> void:
	is_dragging = true


func _on_drag_ended(value_changed: bool) -> void:
	get_tree().current_scene.viewer_node.time = value
	Globalaud.get_node("Music").seek(value)
	is_dragging = false
