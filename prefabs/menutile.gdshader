shader_type spatial;

render_mode blend_mix, unshaded, cull_disabled;

uniform sampler2D texture_albedo; // auto-filled by Godot
uniform sampler2D mask_tex : source_color;       // you assign this once
uniform float dim : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    vec4 col = texture(texture_albedo, UV);
    float mask = texture(mask_tex, UV).r;

    ALBEDO = col.rgb * dim;
    ALPHA = col.a * mask;
}
