from PIL import Image
import os
import sys

def flip_sprite_sheet_preserve_order(image_path, frame_width, frame_height):
    # Cargar la imagen original
    sprite_sheet = Image.open(image_path)
    sheet_width, sheet_height = sprite_sheet.size

    # Calcular cuántos frames hay en la fila
    num_frames = sheet_width // frame_width

    # Crear una nueva imagen para la hoja volteada
    flipped_sheet = Image.new("RGBA", (sheet_width, sheet_height))

    # Procesar cada frame individualmente
    for i in range(num_frames):
        # Recortar el frame
        box = (i * frame_width, 0, (i + 1) * frame_width, frame_height)
        frame = sprite_sheet.crop(box)

        # Voltear el frame
        flipped_frame = frame.transpose(Image.Transpose.FLIP_LEFT_RIGHT)

        # Pegar el frame en su posición original
        flipped_sheet.paste(flipped_frame, (i * frame_width, 0))

    # Construir el nuevo nombre del archivo
    base, ext = os.path.splitext(image_path)
    flipped_path = f"{base}_Flipped{ext}"

    # Guardar la nueva hoja de sprites
    flipped_sheet.save(flipped_path)
    print(f"Imagen volteada (frames conservados) guardada en: {flipped_path}")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Uso: python flip_sprite.py <ruta_al_sprite_sheet>")
    else:
        # Tamaño del frame en tu imagen original: 34x44
        flip_sprite_sheet_preserve_order(sys.argv[1], frame_width=32, frame_height=44)
