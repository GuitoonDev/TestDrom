using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField] private Renderer primitivePrefab = null;
    [SerializeField, Range(2, 100)] private int width = 5;
    [SerializeField, Range(2, 100)] private int height = 5;

    Queue<Transform> wallColumnQueue = new Queue<Transform>();

    ////
    // Alors j'ai voulu profiter du temps qui me restait pour faire en sorte que
    // le mur se générait aussi en éditeur en passant par la méthode "OnValidate"
    // Le résultat n'était pas très satisfaisant...
    // Je pense que j'aurais dû plutôt essayer avec un script WallGeneratorEditor.
    ////

    // [SerializeField, HideInInspector] private int validateWidth;
    // [SerializeField, HideInInspector] private int validateHeight;

    // private void OnValidate() {
    //     if (validateWidth > width) {
    //         int columnsToDestroy = validateWidth - width;
    //         for (int i = 0; i < columnsToDestroy; i++) {
    //             Transform columnItem = wallColumnQueue.Dequeue();
    //             CleanChilds(columnItem);
    //             DestroyImmediate(columnItem.gameObject);
    //         }
    //     }
    //     else if (validateWidth < width) {
    //         int columnsToCreate = width - validateWidth;
    //         for (int i = 0; i < columnsToCreate; i++) {
    //             GameObject newColumn = new GameObject("WallColumn_" + wallColumnQueue.Count);
    //             newColumn.transform.SetParent(transform);
    //             InstantiateColumn(newColumn.transform, validateHeight);
    //             wallColumnQueue.Enqueue(newColumn.transform);
    //         }
    //     }
    //     validateWidth = width;

    //     if (validateHeight > height) {
    //         int primitivesToDestroy = validateHeight - height;
    //         foreach (Transform _columnItem in wallColumnQueue) {
    //             for (int i = 0; i < primitivesToDestroy; i++) {
    //                 DestroyImmediate(_columnItem.GetChild(_columnItem.childCount - i - 1));
    //             }
    //         }
    //     }
    //     else if (validateHeight < height) {
    //         int primitivesToCreate = height - validateHeight;
    //         foreach (Transform _columnItem in wallColumnQueue) {
    //             for (int i = 0; i < primitivesToCreate; i++) {
    //                 InstantiateColumnPrimitive(primitivePrefab, _columnItem, _columnItem.childCount);
    //             }
    //         }
    //     }
    //     validateHeight = height;
    // }

    // private void Awake() {
    //     validateWidth = width;
    //     validateHeight = height;
    // }

    private void Start() {
        GenerateWall();
    }

    public void GenerateWall() {
        CleanChilds(transform);

        // instantiate first column
        GameObject newColumn = new GameObject("WallColumn_0");
        newColumn.transform.SetParent(transform);
        for (int currentHeightIndex = 0; currentHeightIndex < height; currentHeightIndex++) {
            bool isPairWallStep = ((currentHeightIndex % 2) == 0);
            if (isPairWallStep) {
                InstantiateColumnPrimitive(primitivePrefab, newColumn.transform, currentHeightIndex);
            }
        }

        for (int currentWidthIndex = 1; currentWidthIndex < width; currentWidthIndex++) {
            newColumn = new GameObject("WallColumn_" + currentWidthIndex);
            InstantiateColumn(newColumn.transform, height);

            newColumn.transform.SetParent(transform);
            newColumn.transform.position = Vector3.right * currentWidthIndex * primitivePrefab.bounds.size.x;
            wallColumnQueue.Enqueue(newColumn.transform);
        }
    }

    private void InstantiateColumn(Transform _parent, int _columnHeight) {
        for (int currentHeightIndex = 0; currentHeightIndex < height; currentHeightIndex++) {
            InstantiateColumnPrimitive(primitivePrefab, _parent, currentHeightIndex);
        }
    }

    private void InstantiateColumnPrimitive(Renderer _primitive, Transform _parent, int _heightPosition) {
        Renderer newPrimitveRenderer = Instantiate(primitivePrefab, _parent);

        Vector3 newPosition = Vector3.right * (primitivePrefab.bounds.size.x * -0.5f * (_heightPosition % 2));
        newPosition += Vector3.up * _heightPosition * primitivePrefab.bounds.size.y;
        newPrimitveRenderer.transform.position = newPosition;
    }

    private void CleanChilds(Transform _parent) {
        foreach (Transform childItem in _parent) {
            Destroy(childItem.gameObject);
        }
    }
}
