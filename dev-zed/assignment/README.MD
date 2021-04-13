# 문제 해결 방법 (자유롭게 작성)

## 단지에 있는 동의 폴리곤 데이터를 이용하여 런타임에 메쉬를 구성하여 렌더링

### 1. 데이터 읽어오기
1. json 데이터를 읽어오는게 우선으로 판단, json을 유니티에서 읽어오는 방법에 대한 구글링 시작.
2. https://wergia.tistory.com/164의 내용을 참고해 json 데이터를 읽어올 수 있도록 코드 작성 시작.
3. Unity JSON Utility를 이용해 Json타입의 파일을 읽어오고, 파싱하는 기능을 하는 Json이라는 static 클래스를 정의.
4. Sample폴더안의 Json데이터를 분석한 후, API Response Data, Dong Data, Type Data 의 세 가지 클래스를 정의.
    - 동 데이터와 타입 데이터를 추가하면서, 각각의 meta 데이터를 어떻게 추가해야할지에 대한 고민을함. (Key가 한글이었어서 혼란)
    - 딕셔너리로 읽어오려니 Unity JSON Utility에서는 딕셔너리를 지원하지 않는다고 함.
    - 그 때, 이전 회사의 상사가 말했던 일화를 떠올림. 개발하던 프로젝트의 용어가 어려워 클래스의 멤버 이름들을 한글로 지었던 적이 있었다고.
    - 각 클래스에 Meta 클래스를 선언 한 후, 멤버를 한글로 정의.
    - 모든 데이터가 성공적으로 잘 읽어와 지는것 확인.
5. MyFramework 클래스에서 Init 함수를 정의, 해당 위치에서 Json데이터를 읽어오는 코드 작성.
6. 데이터 읽어오기 완료.

### 2. 폴리곤 데이터를 이용해 Mesh 생성
1. MyFramework 클래스에서 1에서 읽어온 데이터를 바탕으로 메시를 생성하는 기능을 하는 Draw함수를 정의.
2. 안내에 나와있는대로, Convert.FromBase64String(), Buffer.BlockCopy() 를 이용해 데이터를 float[] 로 변환.
3. Mesh를 코드를 이용해 실시간으로 만드는 방법에 대한 구글링시작, https://uemonwe.tistory.com/4 블로그를 참고하기로 함.
4. 블로그에서는 vertices의 배열의 길이가 4인 경우에 대한 글이었는데, 과제에 적용해보니 배열길이가 안 맞아서 에러가 발생하고, 생성된 mesh들도 어딘가 이상함.
5. 각 동별 타입별 float[] 데이터들을 Vector3로 변환한 후 리스트에 넣고, 각 리스트들의 Count를 print 해 보니 모두 6의 배수인것을 확인. vertices의 배열의 길이가 6인 Mesh를 생성해야한다는것을 깨닫게 됨.
7. 정점이 6인 경우의 삼각형이 어떤식으로 그려지는지에 대한 구글링, http://grasshopperprimer.com/en/1-foundations/1-6/1_What%20is%20a%20Mesh.html 사이트 글을 참고해 vertices와 triangles의 값을 세팅.
8. 단지별로 mesh가 생성, 구성된것 확인.


## 문제1의 결과를 바탕으로 생성된 메쉬에 텍스쳐를 매핑
