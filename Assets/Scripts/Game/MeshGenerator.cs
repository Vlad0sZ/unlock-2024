using UnityEngine;
using UnityEngine.Rendering;

public class MeshGenerator
{
    private readonly float _width;      // Ширина меша
    private readonly float _height;     // Высота меша
    private readonly float _depth;      // Глубина меша

    public MeshGenerator(float width, float height, float depth)
    {
        _width = width;
        _height = height;
        _depth = depth;
    }

    public Mesh GenerateCutoutMesh(int[] data, int width, int height)
    {
        // Получаем данные пикселей текстуры
        var pixels = data;
        int texWidth = width;
        int texHeight = height;

        // Вершины и треугольники для создания меша
        var vertices = new System.Collections.Generic.List<Vector3>();
        var triangles = new System.Collections.Generic.List<int>();

        // Соотношение сторон меша и текстуры
        float stepX = _width / texWidth;
        float stepY = _height / texHeight;

        // Индексы вершин
        int vertexIndex = 0;

        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                // Определяем, является ли пиксель белым (вырез)
                if (pixels[y * texWidth + x] != 0)
                {
                    float posX = x * stepX - _width / 2;
                    float posY = y * stepY - _height / 2;

                    // Передняя грань
                    vertices.Add(new Vector3(posX, posY, _depth / 2)); // Нижняя левая
                    vertices.Add(new Vector3(posX + stepX, posY, _depth / 2)); // Нижняя правая
                    vertices.Add(new Vector3(posX + stepX, posY + stepY, _depth / 2)); // Верхняя правая
                    vertices.Add(new Vector3(posX, posY + stepY, _depth / 2)); // Верхняя левая

                    // Задняя грань
                    vertices.Add(new Vector3(posX, posY, -_depth / 2)); // Нижняя левая (зад)
                    vertices.Add(new Vector3(posX + stepX, posY, -_depth / 2)); // Нижняя правая (зад)
                    vertices.Add(new Vector3(posX + stepX, posY + stepY, -_depth / 2)); // Верхняя правая (зад)
                    vertices.Add(new Vector3(posX, posY + stepY, -_depth / 2)); // Верхняя левая (зад)



                    // Передние треугольники
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 3);

                    // Задние треугольники
                    triangles.Add(vertexIndex + 4);
                    triangles.Add(vertexIndex + 6);
                    triangles.Add(vertexIndex + 5);
                    triangles.Add(vertexIndex + 4);
                    triangles.Add(vertexIndex + 7);
                    triangles.Add(vertexIndex + 6);


                    if (x - 1 >= 0 && pixels[y * texWidth + x - 1] == 0)
                    {
                        // Боковые грани (левая сторона)
                        triangles.Add(vertexIndex);
                        triangles.Add(vertexIndex + 7);
                        triangles.Add(vertexIndex + 4);
                        triangles.Add(vertexIndex);
                        triangles.Add(vertexIndex + 3);
                        triangles.Add(vertexIndex + 7);
                    }

                    if (x + 1 < texWidth && pixels[y * texWidth + x + 1] == 0)
                    {
                        // Боковые грани (правая сторона)
                        triangles.Add(vertexIndex + 1);
                        triangles.Add(vertexIndex + 6);
                        triangles.Add(vertexIndex + 2);
                        triangles.Add(vertexIndex + 1);
                        triangles.Add(vertexIndex + 5);
                        triangles.Add(vertexIndex + 6);
                    }
                    if (y + 1 < texHeight && pixels[(y + 1) * texWidth + x] == 0)
                    {
                        // Боковые грани (верхняя сторона)
                        triangles.Add(vertexIndex + 3);
                        triangles.Add(vertexIndex + 6);
                        triangles.Add(vertexIndex + 7);
                        triangles.Add(vertexIndex + 3);
                        triangles.Add(vertexIndex + 2);
                        triangles.Add(vertexIndex + 6);
                    }
                    if (y - 1 >= 0 && pixels[(y - 1) * texWidth + x] == 0)
                    {
                        // Боковые грани (нижняя сторона)
                        triangles.Add(vertexIndex);
                        triangles.Add(vertexIndex + 5);
                        triangles.Add(vertexIndex + 1);
                        triangles.Add(vertexIndex);
                        triangles.Add(vertexIndex + 4);
                        triangles.Add(vertexIndex + 5);
                    }

                    // Индекс вершин увеличивается на 8 (передняя + задняя грань)
                    vertexIndex += 8;
                }
            }
        }

        // Создаем меш
        var mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32; 
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh; 
    }
}
