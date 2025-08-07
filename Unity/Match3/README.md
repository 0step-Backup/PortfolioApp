## Match3

Untiy로 개발한 Match3 게임 프트폴리오 앱

![cover](https://raw.githubusercontent.com/0step-Backup/PortfolioApp/refs/heads/main/Unity/Match3/%23%20public/cover.png)

### 개발 환경
- Unity (2022.3.38f1)

### 개요
- Tilemap 사용한 스테이지 구성
- [itch.io WebGL 배포 (브라우저 실행)](https://scv-mech.itch.io/match-3)

### 주요 로직
- [LevelDesign.cs](https://github.com/0step-Backup/PortfolioApp/blob/main/Unity/Match3/Assets/Scripts/LevelDesign.cs)
  - Tilemap의 Grid에서 타일 정보 로드
  - Gem 오브젝트 생성을 위해 Stage에 정보 전달
  - 추가 생성 위치 정보 로드
- [Stage.cs](https://github.com/0step-Backup/PortfolioApp/blob/main/Unity/Match3/Assets/Scripts/Stage/Stage.cs): 스테이지 처리
  - [Stage.MatchChecker.cs](https://github.com/0step-Backup/PortfolioApp/blob/main/Unity/Match3/Assets/Scripts/Stage/Stage.MatchChecker.cs): 매치 감지, 연쇄 반응, 젬 드롭 및 생성
  - [Stage.InputProcessor.cs](https://github.com/0step-Backup/PortfolioApp/blob/main/Unity/Match3/Assets/Scripts/Stage/Stage.InputProcessor.cs): 젬 교체 및 선택을 위한 사용자 입력 처리
- [Gem.cs](https://github.com/0step-Backup/PortfolioApp/blob/main/Unity/Match3/Assets/Scripts/Node/Gem.cs)
  - 스와이프, 드랍 등의 이동 처리(DOTwen 사용)
  - OnMouseXXX 이벤트 처리
    - BoxCollider2D

### 추가 패키지
- [DOTween Pro](https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416): 이동 등의 효과를 위한 애니메이션 시스템
- [ayellowpaper.Serialized Dictionary](https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052): 인스펙터 편집을 위한 직렬화 가능 사전