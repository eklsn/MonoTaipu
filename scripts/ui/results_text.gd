extends RichTextLabel

@export var startposdiff : Vector2
@export var startalpha : float = 0
@export var speed : float = 0.05
@export var autostart : bool

var endalpha
var endpos
var emitting : bool = false
signal fin
func _ready() -> void:
	visible=false
	if autostart:
		start()

func start():
	visible=true
	endalpha=modulate.a
	endpos=position
	position=position+startposdiff
	modulate.a=startalpha
	emitting=true
func _process(delta: float) -> void:
	if emitting:
		modulate.a = lerp(modulate.a,endalpha,15*delta)
		position= lerp(position,endpos,15*delta)
	if (round(position)==endpos && snapped(endalpha,0.1)==endalpha) && emitting:
		emitting=false
		fin.emit()
