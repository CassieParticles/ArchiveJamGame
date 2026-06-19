using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SecondOrderMovement<>))]
public class SecondOrderDrawer : PropertyDrawer {

    public VisualTreeAsset InspectorUXML;
    VisualElement graph;

    public float graphSamplePeriod = 0.02f;
    public int graphSampleCount = 80;
    public float graphOffsetY = 0.1f; //In percentage

    private float f, z, r;

    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        VisualElement myInspector = new VisualElement();

        //Load the UXML file of the property
        if (InspectorUXML != null) {
            VisualElement uxmlContent = InspectorUXML.CloneTree();
            myInspector.Add(uxmlContent);
        }

        //Get references to the property elements
        graph = myInspector.Q<VisualElement>("graphContainer");
        Slider freqField = myInspector.Q<Slider>("Frequency");
        Slider zetaField = myInspector.Q<Slider>("Zeta");
        Slider responseField = myInspector.Q<Slider>("Response");

        //Set the graph to draw when repainting
        graph.generateVisualContent += DrawGraph;
        graph.MarkDirtyRepaint();


        //Update the Graph when any of the 3 float variables change
        f = property.FindPropertyRelative("frequency").floatValue;
        z = property.FindPropertyRelative("zeta").floatValue;
        r = property.FindPropertyRelative("response").floatValue;

        freqField.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            property.FindPropertyRelative("frequency").floatValue = evt.newValue;
            f = property.FindPropertyRelative("frequency").floatValue;
            property.serializedObject.ApplyModifiedProperties();
            graph.MarkDirtyRepaint();
        });

        zetaField.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            property.FindPropertyRelative("zeta").floatValue = evt.newValue;
            z = property.FindPropertyRelative("zeta").floatValue;
            property.serializedObject.ApplyModifiedProperties();
            graph.MarkDirtyRepaint();
        });

        responseField.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            property.FindPropertyRelative("response").floatValue = evt.newValue;
            r = property.FindPropertyRelative("response").floatValue;
            property.serializedObject.ApplyModifiedProperties();
            graph.MarkDirtyRepaint();
        });

        return myInspector;
    }

    private void DrawGraph(MeshGenerationContext ctx) {        
        SecondOrderDynamics<float> func = SecondOrderFactory.Float(f, z, r, -1);

        var rect = ctx.visualElement.contentRect;
        if (rect.width <= 0 || rect.height <= 0)
            return;

        Painter2D painter = ctx.painter2D;
        painter.lineWidth = 1;
        painter.strokeColor = UnityEngine.Color.cyan;

        int adjustedSampleCount = (int)(graphSampleCount * f / 2);

        Vector2[] points = new Vector2[adjustedSampleCount];

        float xMax = adjustedSampleCount;
        float xScale = rect.width / xMax;
        float xStart = xScale * 3;
        float yScale = rect.height / 2f;
        float yOffset = rect.center.y - rect.height * graphOffsetY;
        float yBottom = yOffset + yScale;

        float yMax = 0f;
        float yMin = -1f;

        for (int i = 0; i < adjustedSampleCount - 2; i++) {
            float t = i / (float)(adjustedSampleCount);
            float x = Mathf.Lerp(0, xMax * xScale, t);
            float y = func.UpdateMovement(graphSamplePeriod, 0);

            if (y < yMin) {
                yMin = y;
            } else if (y > yMax) {
                yMax = y;
            }

            points[i] = new Vector2(x, y);
        }

        float yRange = (yMax - yMin);
        float yStart = yOffset - ((points[0].y) / yRange) * yScale;

        //Make the graph axis
        painter.strokeColor = UnityEngine.Color.grey;
        painter.BeginPath();
        painter.MoveTo(new Vector2(rect.x, yOffset));
        painter.LineTo(new Vector2(rect.x + rect.width, yOffset));
        painter.Stroke();


        //The function itself
        painter.strokeColor = UnityEngine.Color.cyan;
        painter.BeginPath();
        painter.MoveTo(new Vector2(rect.x, yStart));

        painter.LineTo(new Vector2(rect.x + xStart, yStart));

        for (int i = 0; i < points.Length - 2; i++) {
            float adjustedY = (points[i].y) / yRange;

            Vector2 adjustedPoint = new Vector2((points[i].x + rect.x) + xStart,
                                    yOffset - adjustedY * yScale);

            painter.LineTo(adjustedPoint);
        }
        painter.Stroke();
    }

}

