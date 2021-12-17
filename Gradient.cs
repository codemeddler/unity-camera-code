using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Gradient : MonoBehaviour
{
	public Color lightColor;
	public Color darkColor;
	public Color darkestColor;
	public int gradientLayer = 7;
	public float stepSize = 0.05f;
	
	Color topColor;
	Color bottomColor;
	Color topTargetColor;
	Color bottomTargetColor;

	Mesh gradientMesh;

	void Awake()
	{
		gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
		
		GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
		GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << gradientLayer);
		
		Camera gradientCamera = new GameObject("Gradient Camera", typeof(Camera)).GetComponent<Camera>();
		gradientCamera.depth = GetComponent<Camera>().depth - 1;
		gradientCamera.cullingMask = 1 << gradientLayer;

		gradientMesh = new Mesh();
		gradientMesh.vertices = new Vector3[4] { new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, -.577f, 1f), new Vector3(100f, -.577f, 1f) };
		gradientMesh.colors = new Color[4] { lightColor, lightColor, darkColor, darkColor };

		topColor = lightColor;
		bottomColor = darkColor;
		topTargetColor = lightColor;
		bottomTargetColor = darkColor;
		gradientMesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };

		Material gradientMaterial = new Material("Shader \"Vertex Color Only\"{Subshader{BindChannels{Bind \"vertex\", vertex Bind \"color\", color}Pass{}}}");

		GameObject BackGround = new GameObject("BackGround", typeof(MeshFilter), typeof(MeshRenderer));

		((MeshFilter)BackGround.GetComponent(typeof(MeshFilter))).mesh = gradientMesh;
		BackGround.GetComponent<Renderer>().material = gradientMaterial;
		BackGround.layer = gradientLayer;
	}

	public void DeepenGradient()
	{
		topTargetColor = bottomColor;

		bottomTargetColor.r -= stepSize;
		if (bottomTargetColor.r < darkestColor.r)
			bottomTargetColor.r = darkestColor.r;
		
		bottomTargetColor.g -= stepSize;
		if (bottomTargetColor.g < darkestColor.g)
			bottomTargetColor.g = darkestColor.g;
		
		bottomTargetColor.b -= stepSize;
		if (bottomTargetColor.b < darkestColor.b)
			bottomTargetColor.b = darkestColor.b;

		topTargetColor.a = 1.0f;
		bottomTargetColor.a = 1.0f;
	}

	public void LightenGradient()
	{
		bottomTargetColor = darkColor;
		topTargetColor = lightColor;
	}

	void MaintainGradient()
	{
		bool bChangedColors = false;

		if (topColor != topTargetColor)
		{
			topColor = Color.Lerp(topColor, topTargetColor, Time.deltaTime * stepSize);
			bChangedColors = true;
		}

		if (bottomColor != bottomTargetColor)
		{
			bottomColor = Color.Lerp(bottomColor, bottomTargetColor, Time.deltaTime * stepSize);
			bChangedColors = true;
		}

		if (bChangedColors)
		{
			Color[] colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };
			gradientMesh.colors = colors;
		}
	}
	
	void Update()
	{
		MaintainGradient();
	}
}