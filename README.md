# 2022 창의 인재 동반 사업 인디 게임 개발자 양성 멘토링 과정 수료
![image](https://github.com/HyunDongHo/ProjectB_2_1/assets/46379443/389773d4-3fcb-4765-ad32-97a608ff5360)

- 창의인재동반사업에 멘티로서 참여하여 심플 하고 타격감이 있는 3D 방치형 키우기 게임을 제작 하였습니다. 멘토링을 통해 해당 장르에 필요한 시스템에서 우선순위를 정해 개발을 진행 할 수 있었습니다. 그리고 컨텐츠 기획 및 개발 부분에 피드백을 받을 수 있어서 유저의 재미와 게임 내 편리성을 제공 할 수 있도록 노력하였습니다.
- 멘토 : 권오현 (뒤끝서버 대표님)
- 프로그램 기간 : 2022.05 ~ 2022.11
- 멘토링 기간 : 2022.05 ~ 2023.05

# ProjectB_2_1

# 프로젝트 소개 
- 3D 방치형 3총사 키우기 게임
- 스테이지 내의 몬스터를 사냥하면서 얻은 재화로 3가지 종류의 캐릭터 모두를 성장 할 수 있는 게임

# 팀 프로젝트 개요 
- 제작기간 : 11개월
- 개발도구 : Unity
- 버전 : 2021.3.4f1
- 개발언어 : C#
- 플랫폼 : 모바일
- 팀원 : 2명

# 참여 부분
1. 게임 플레이 프로그래밍
  - Unity엔진을 활용해 캐릭터 스킬 구현 


2. UI/UX 프로그래밍
  - UGUI를 사용해 게임 UI 구현 
  - 뒤끝서버 차트 연동 및 유저 데이터 저장 
  -  UI 자동화 및 동적 스크롤 구현 
  - 성장, 장비, 동료, 미지의 탑 등 캐릭터 성장에 필요한 컨텐츠 개발
  - 스킬, 스테이지 이동, 퀘스트, 미션, 광고 버프 시스템 개발 
  - DoTween을 사용한 UI 애니메이션 구현  

# 영상 및 멘토링 내용
- 월 별 구현 영상 및 멘토링 내용 노션 링크 : https://sassy-athlete-7a6.notion.site/2022-4858ea27e0314e8b8d708de1b6a486aa?pvs=4

# 회고록 
- 프로젝트에서 주로 UI 컨텐츠를 많이 제작을 하였는데, 만들면서 어려웠거나 잘 몰랐었던 부분
  1. 뒤끝서버 차트를 연동하여 차트에 있는 데이터를 불러와서 UI 팝업창을 열었을 때 UI를 세팅하는 부분이다. UI 팝업창 게임오브젝트가 활성화 됬을 때 서버에 있는 데이터를 불러오고 처리 하니까 팝업창 키는 속도가 좀 느리는 문제가 있었다. 따라서 구글  링을 통해서 찾아본 결과 내가 유니티 생성 주기를 잘 모르고 있었다. 따라서 차트에 있는 데이터는 인게임이 시작하고 UI 팝업창이 켜지지 않는 상태일 때, Awake함수를 사용해 미리 팝업창의 정보를 세팅을 해놓았다.
  2. 실시간으로 텍스트 UI 정보를 업데이트 할 때 Update 문을 사용해서 처음에 개발을 진행 하였는데, 나중에 UI 에서는 update문을 가급적 사용을 피해야 된다고 알게 되어서 해당 컨텐츠에 필요한 매니저를 생성해서 정말 update가 필요할 때 static 함수를 통해 업데이트를 하도록 개발 하였고 시간의 흐름에 관련된 기능을 구현 할 때에는 코루틴을 사용해서 개발을 진행하였다.
  3. UI 연출을 구현 할 때(ex, 크기 조절, 위치 이동)은 처음에는 update문을 사용하여 transform 컴포넌트를 수정하는 방식이나 Animation 녹화 방식으로 개발을 진행 하였는 데, 이러한 방식 보다는 Dotween 방식으로 UI Animation을 구현 하는 게 Update문을 가급적 줄일 수 있을 뿐만 아니라 Dotween 안에 시간 조절이나 여러 기능들이 많이 제공하기 떄문에 단순한 방식으로 개발 하기 보다는 "이것 보다 더 나은 방법이 없을 까?" 고민하면서 개발을 진행 하였다.
  4. 동적 스크롤을 구현 할 때, content 부모 아래 자식 객체가 너무나 많으면 스크롤 시 프레임이 급격히 떨어지는 현상을 보게 되었다. 처음에는 이것을 어떻게 해결해야 할 지 막막 했다. 유니티 코리아 유튜브 채널에 UI 최적화 영상을 보게 되었는 데 이때 UI도 메시로 구성된 사실을 처음 알았었고 동적스크롤 파트에 UI pooling 기법을 소개 해주셨다. content 내 자식겍체를 소수로 두고 스크롤 시 오브젝트 안의 컴포넌트를 업데이트 한다는 설명을 듣고 몬스터 생성 시 오브젝트 풀링을 사용하는 줄 알았는 데 "UI도 최적화를 위해 오브젝트 풀링이 사용이 되는 구나" 라는 사실을 깨달았고 많은 컨텐츠를 제작 중 늦게 ui최적화라는 매뉴얼을 보게 되어 좀 아쉬움이 남았었다.
  5. 컨텐츠를 많이 만들 수록 그로 인해서 약간의 버그가 발생하는 데 이를 처음에는 로그를 남겨 문제를 분석하였는데, 팀원의 조언을 받아 디버깅을 통해 문법적, 논리적 실수들을 깨달을 수 있었고 빨리 문제를 해결 할 수 있었다. 그리고 디버깅을 습관화 하도록 노력하였다. 
  6. UI 컨텐츠 기능을 구현할 때 처음에는 막막 했는데 여러 레퍼런스 될만한 게임을 플레이 해보고 어떻게 클릭이 되고 UI로 표시 되는 지 분석 후 개발을 진행하였다. 예를 들어 스테이지 팝업창 클릭 시 현재 플레이 되고 있는 스테이지 정보나 보상 목록들이 보이는데 다른 스테이지 클릭 했을 때 어떻게 표시 되어야 유저의 편의성을 개선 할 수 있을 지 고민이 되었다. 이때, "광전사 키우기" 라는 게임에 스테이지 선택 시 UI update 되는 부분들을 분석 후 이를 단위 별로 나눠서 차근차근 구현해 나갔다. 따라서 처음에는 막막하더라도 여러 레퍼런스를 찾아서 어떻게 알고리즘을 짯는지 분석하는 과정이 개발에 있어서 도움이 많이 되었다.
  7. 뒤끝 서버에서 불러온 데이터를 대게 key와 value로 구성된 Dictionary 라는 자료구조를 사용하여 readonly 이라는 런타임 상수로 불러온 다음, 이를 게임 내 쓸 Dictionary에 넣어 두고 이를 Getter 함수의 return 타입을 받아서 사용 했었는 데, 처음에는 버그가 나지 않아서 별 문제가 없다고 생각을 하였는 데 문득 만약 협업을 하게 됬을 때 이 게임 내 Dictionary는 차트에 있는 데이터라서 수정하면 안되는 데 인텔리젼스 기능으로 수정 함수를 쓰게 된다면 협업 시 불상사가 발생 할 거 같았다. 따라서 c# 강의나 유튜브를 통해서 이러한 고민을 해결 할 수 있는 부분을 찾아보았고 IEnumerable 이라는 인터페이스를 사용하면 인텔리션스 기능에 수정하는 함수는 안보이고 읽기로서 사용 할 수 있다는 사실과 더불어 Dictionary 안을 들어가보면 IEnumerable이라는 인터페이스를 구현한 클래스이기 떄문에 Getter에 return 타입에 사용할 수 있는 부분도 알 수 있었다. 따라서 C# 공부의 중요성을 알게 되었고 현재 "Effective C#" 책을 구매 하여 더 깊게 언어를 공부 해야 겠다고 다짐했다.
      
       

