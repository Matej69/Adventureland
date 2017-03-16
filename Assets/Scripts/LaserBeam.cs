using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserBeam : Item {

    public GameObject prefab_beamPart;

    public Transform startPos;
    public Transform endPos;

    List<GameObject> parts = new List<GameObject>();

    Vector3 dir;

    Vector3 lastPos;

    // Use this for initialization
    void Start () {
        lastPos = startPos.position;
        dir = (endPos.position - startPos.position).normalized;

        CreateBeamPart();
    }
	
	// Update is called once per frame
	void Update () {

       
            Reset();
            CreateBeamPart();
        

    }


    void PointYToDir(GameObject go, Vector3 _dir)
    {
        Quaternion toRotation = Quaternion.FromToRotation(go.transform.up, _dir);
        go.transform.rotation = toRotation;
    }

    void RandomRotatePart(GameObject go, float _maxRot)
    {
        Vector3 rotateBy = new Vector3(Random.Range(0, _maxRot), Random.Range(0, _maxRot), Random.Range(0, _maxRot));
        go.transform.Rotate(rotateBy);
    }

    void CreateBeamPart()
    {
        bool stopCreating = false;
        while (!stopCreating)
        {
            GameObject newPart = (GameObject)Instantiate(prefab_beamPart, lastPos, Quaternion.identity);
            Vector3 newPartDir = (endPos.position - newPart.transform.position).normalized;
            PointYToDir(newPart, newPartDir);
            RandomRotatePart(newPart, 12);
            newPart.transform.SetParent(transform);
            parts.Add(newPart);

            lastPos = newPart.transform.FindChild("Mesh").transform.FindChild("End").transform.position;
            if (IsEndReached())
                stopCreating = true;
        }
    }

    public void SetStartPos(Vector3 _pos)
    {
        startPos.position = _pos;
    }
    public void SetEndPos(Vector3 _pos)
    {
        endPos.position = _pos;
    }

    private bool IsEndReached()
    {
        return (Vector3.Distance(endPos.position, lastPos) < 0.1f);
    }
    
    private void Reset()
    {        
        for(int i = parts.Count - 1; i >= 0; --i)
        {
            Destroy(parts[i]);
            parts.RemoveAt(i);            
        }

        lastPos = startPos.position;
    }

    public void RecreateBeam()
    {
        Reset();
        CreateBeamPart();
    }


}
