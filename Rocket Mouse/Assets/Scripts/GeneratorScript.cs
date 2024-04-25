using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    // �߰��� �� �������� �����ϴ� �迭
    public GameObject[] availableRooms;
    // ���� ���ӿ� ������ �� ������Ʈ ����Ʈ
    public List<GameObject> currentRooms;

    //ȭ���� ���� ũ��(���� : ����)
    float screenWidthInPoints;
    // �ٴ� ������Ʈ �̸�
    const string floor = "Floor";

    // �߰��� ������Ʈ �������� �����ϴ� �迭
    public GameObject[] availableObjects;
    // ���� ���ӿ� ������ ����, ������ ������Ʈ ����Ʈ
    public List<GameObject> objects;
    // ������Ʈ�� �ּ� ����
    public float objectMinDistance = 5f;
    // ������Ʈ�� �ִ� ����
    public float objectMaxDistance = 10f;
    // ������Ʈ Y�� ��ġ �ּҰ�
    public float objectMinY = -1.4f;
    // ������Ʈ Y�� ��ġ �ִ�
    public float objectMaxY = 1.4f;
    // ������Ʈ �ּ� ȸ����
    public float objectMinRotation = -45f;
    // ������Ʈ �ִ� ȸ����
    public float objectMaxRotation = 45f;

    public int range;

    private void Start()
    {
        // ī�޶��� ������ ���� 2��� ���� ���� ũ�� ���
        float height = 2.0f * Camera.main.orthographicSize;     // height : 6.4
        // ���� ũ�⿡ ȭ�� ������ ���ؼ� ���� ũ�� ���
        screenWidthInPoints = height * Camera.main.aspect;      // aspect = ���� ũ�⸦ 1�� �������� ���� ũ���� ����

        range =  availableObjects.Length;
    }

    private void Update()
    {
        GeneralRoomIfReauired();
        GenerateObjectsIfRequired();
    }
    private void AddRoom(float farthestRoomEndX)
    {
        // �� �����յ� �� �ϳ��� ����
        int randonRoomIndex = Random.Range(0, availableRooms.Length);
        // ���õ� ���� �߰�
        GameObject room = Instantiate(availableRooms[randonRoomIndex]);
        // ���� ���� ũ��
        float roomWidth = room.transform.Find(floor).localScale.x;
        // ���� �߾� ��ġ
        float roomCenter = farthestRoomEndX + roomWidth / 2;
        // ���� ���� ��ġ�� ������Ʈ�� ��ġ��Ŵ
        room.transform.position = new Vector3(roomCenter, 0, 0);
        // �߰��� ���� ���� �߰��� �� ����Ʈ�� �߰�
        currentRooms.Add(room);
    }

    private void GeneralRoomIfReauired()
    {
        // ������ ���� ����� �����ϴ� ����Ʈ
        List<GameObject> roomsToRemove = new List<GameObject>();
        // ���� �����ӿ� ���� �������� ����
        bool addRooms = true;

        // ���콺 ������Ʈ�� x�� ��ġ
        float playerX = transform.position.x;
        // ������ ���� ���� ��ġ�� ����
        float removeRoomX = playerX - screenWidthInPoints;
        // �߰��� ���� ���� ��ġ�� ����
        float addRoomX = playerX + screenWidthInPoints;
        // ���� �����ʿ� ��ġ�� ���� �����ʳ� ��ġ
        float farthestRoomEndX = 0f;

        // ���� �߰��� ���� �ϳ��� ó��
        foreach (var room in currentRooms)
        {
            // room ������Ʈ�� �ٴڿ�����Ʈ�� ã�� ����ũ�⸦ ������
            float roomWidth = room.transform.Find(floor).localScale.x;
            // ���� ��ġ���� ���� ũ���� ������ �� ���� �� ��ġ�� ���
            float roomStartX = room.transform.position.x - roomWidth / 2;
            // ���� ���� �� ��ġ���� ���� ũ�⸦ ���� ������ �� ��ġ�� ���
            float roomEndX = roomStartX + roomWidth;

            // ���� �Ѱ��� ���� �� ��ġ�� ���߰� ���� ��ġ���� �����ʿ� ������ �� �߰�x
            if (roomStartX > addRoomX)
                addRooms = false;

            // �� ���� ���� ��ġ���� ���ʿ� �����ϴ� ���� ������ ����� ��Ͽ� �߰�
            if (roomEndX < removeRoomX)
                roomsToRemove.Add(room);

            // ���� ������ ���� ������ �� ��ġ�� �ִ� �޼ҵ带 �̿��Ͽ� ����
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        // ������ �� ������Ʈ�� �ϳ��� �����ϸ鼭
        foreach (var room in roomsToRemove)
        {
            // ����Ʈ���� ����
            currentRooms.Remove(room);
            // ������Ʈ ����
            Destroy(room);
        }

        // ���� �߰� �ؾ��Ѵٸ� ���� �߰�
        if (addRooms)
            AddRoom(farthestRoomEndX);
    }

    private void AddObject(float lastObjectX)
    {
        // �߰��� ������Ʈ�� �ε����� �������� ����
        int randomIndex = Random.Range(0, range);
        // �������� ���� �ε��� ��ȣ�� ������Ʈ�� ����
        GameObject obj = Instantiate(availableObjects[randomIndex]);
        // ���ο� ������Ʈ�� X�� ��ġ�� ���
        float objectPositionX =
            lastObjectX + Random.Range(objectMinDistance, objectMaxDistance);
        // ���ο� ������Ʈ�� Y�� ��ġ�� ���
        float randomY = Random.Range(objectMinY, objectMaxY);

        // ���� ��ġ���� ������Ʈ�� ��ġ�� ����
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        // �������� ȸ�� �� ���
        float rotation = Random.Range(objectMinRotation, objectMaxRotation);

        // ���� ȸ�� ���� ���ʹϾ����� ������Ʈ�� ȸ���� ����
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
        // ������Ʈ ����Ʈ�� �߰�
        objects.Add(obj);
    }

    private void GenerateObjectsIfRequired()
    {
        // �÷��̾��� X�� ��ġ
        float playerX = transform.position.x;
        // ������ ������Ʈ�� ���� ��ġ
        float removeObjetX = playerX - screenWidthInPoints;
        // �߰��� ������Ʈ�� ���� ��ġ
        float addObjectX = playerX + screenWidthInPoints;
        // ���� �����ʿ� ��ġ�� ������Ʈ�� x�� ��ġ
        float farthestObjectX = 0;

        // ������ ������Ʈ ����Ʈ
        List<GameObject> objectsToRemove = new List<GameObject>();

        // ���� �߰��Ǿ� �ִ� ������Ʈ���� �ϳ��� ó��
        foreach (var obj in objects)
        {
            // ������Ʈ�� X�� ��ġ
            float objX = obj.transform.position.x;
            // �ִ밪 �������� ���� �����ʿ� ��ġ�� ������Ʈ�� ��ġ�� ����
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            // ������Ʈ ��ġ�� ���� ���� ��ġ���� �����̸� ������Ʈ ���� ����Ʈ�� �߰�
            if (objX < removeObjetX)
                objectsToRemove.Add(obj);
        }

        // ���� ����Ʈ�� �߰��� ������Ʈ�� ��� ����
        foreach (var obj in objectsToRemove)
        {
            // ����Ʈ���� ����
            objects.Remove(obj);
            // ������Ʈ ����
            Destroy(obj);
        }
        // ���� �����ʿ� ��ġ�� ������Ʈ�� �߰� ���� ��ġ���� �����̸� ���ο� ������Ʈ ����
        if (farthestObjectX < addObjectX)
            AddObject(farthestObjectX);
    }
}
