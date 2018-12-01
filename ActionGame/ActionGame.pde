float centiWidth, centiHeight;

float playerPosX;
float playerPosY;
float playerScaleX = 20;
float playerScaleY = 20;
float playerScaleY_default = 20;
float playerScaleY_down = 10;

float speed = 5.0;

boolean cannotMove = false;

float jumpPower;
boolean isJumping = true;
boolean isSecondJumping = false;
float gravity = 9.8;

float defaultAttackSpeed = 50.0;
float attackSpeed;
int attackCount;
int attackCountSpan = 10;

boolean leftMove = false;
boolean rightMove = false;
boolean upMove = false;
boolean downMove = false;
boolean jumpMove = false;
boolean attackMove = false;
boolean secondJumpMove = false;

boolean isRighting = true;

Scene scene01;
Scene scene02;
Scene scene03;

MoveNeedle mn01;
MoveNeedle mn02;

Scene nowScene;

float spawnPosX, spawnPosY;
Scene spawnScene;

boolean debugMode = false;

final float TARGET_FPS = 60.0f;
final float FRAME_TIME = 1000.0f / TARGET_FPS;
int lastUpdateTime = 0;
float elapsedTime = 0.0f;

void setup() {
  //size(1728, 972);
  size(1600, 900);
  centiWidth = width / 100.0;
  centiHeight = height / 100.0;

  background(255);

  rectMode(CENTER);
  ellipseMode(CENTER);
  textAlign(CENTER, CENTER);

  playerPosX = 50;
  playerPosY = centiHeight * 80;

  scene01 = new Scene("Stage01");
  scene02 = new Scene("Stage02");
  scene03 = new Scene("Stage03");

  scene01.addBlock(new Block(width/2, height-10, width, 20));
  scene01.addBlock(new Block(10, height/2, 20, height));
  scene01.addBlock(new Block(width - 10, height/2, 20, height));

  //scene01.addBlock(new Block(340, 855, 50, 50));

  //scene01.addBlock(new Block(1320, 855, 50, 50));

  for (int i = 0; i < 3; i++) {
    scene01.addBlock(new Block(340 + i * 150, 855 - i * 50, 150, 50));
  }
  for (int i = 0; i < 3; i++) {
    scene01.addBlock(new Block(740 + i * 50, 655 - i * 150, 50, 150));
  }

  scene01.addBlock(new Block(915, 305, 100, 50));

  for (int i = 0; i < 5; i++) {
    scene01.addBlock(new Block(990 + i * 100, 275 + i * 100, 50, 50));
  }

  for (int i = 0; i < 33; i++) {
    scene01.addNeedle(new Needle(430 + i * 30, 860, 20, 20));
  }
  scene01.addDoor(new Door(1500, 840, 50, 50, scene02));

  scene01.addSave(new Save(920, 265, 30, 30));
  scene01.addBlock(new Block(950, 265, 30, 30));

  scene01.addBlock(new Block(100, 800, 20, 20));
  scene01.addBlock(new Block(120, 800, 20, 20));
  scene01.addBlock(new Block(140, 800, 20, 20));
  scene01.addBlock(new Block(100, 780, 20, 20));
  scene01.addBlock(new Block(140, 780, 20, 20));

  mn01 = new MoveNeedle(890, 405, 20, 20, 90, 400, 60);
  mn01.setTrap(740, 405, 50, 50, scene01);
  scene01.addMoveNeedle(mn01);

  mn01 = new MoveNeedle(1420, 860, 20, 20, 1420, 630, 20);
  mn01.setTrap(1342, 525, 50, 50, scene01);
  scene01.addMoveNeedle(mn01);

  scene02.addBlock(new Block(width/2, height-10, width, 20));
  scene02.addBlock(new Block(10, height/2, 20, height));
  scene02.addBlock(new Block(width - 10, height/2, 20, height));

  //scene02.addBlock(new Block(500, 135, 100, 20));

  scene02.addDoor(new Door(1500, 840, 50, 50, scene01));

  scene02.addBlock(new Block(1100, 500, 20, 800));

  scene02.addBlock(new Block(1160, 710, 100, 20));
  scene02.addBlock(new Block(1260, 560, 100, 20));
  scene02.addBlock(new Block(1160, 410, 100, 20));
  scene02.addBlock(new Block(1260, 260, 100, 20));
  scene02.addBlock(new Block(1160, 140, 100, 20));

  scene02.addSave(new Save(1125, 115, 30, 30));
  scene02.addSave(new Save(225, 765, 30, 30));

  scene02.addBlock(new Block(1060, -11, 300, 20));

  scene02.addBlock(new Block(900, 400, 20, 800));

  scene02.addBlock(new Block(540, 790, 700, 20));

  scene02.addBlock(new Block(200, 430, 20, 700));

  scene02.addBlock(new Block(40, 700, 40, 40));
  scene02.addBlock(new Block(105, 550, 40, 40));
  scene02.addBlock(new Block(170, 400, 40, 40));
  scene02.addBlock(new Block(40, 250, 40, 40));
  scene02.addBlock(new Block(170, 100, 40, 40));

  scene02.addNeedle(new Needle(40, 735, 20, 20));
  scene02.addNeedle(new Needle(105, 585, 20, 20));
  scene02.addNeedle(new Needle(170, 435, 20, 20));
  scene02.addNeedle(new Needle(40, 285, 20, 20));
  scene02.addNeedle(new Needle(170, 135, 20, 20));

  scene02.addBlock(new Block(790, 90, 200, 20));

  scene02.addBlock(new Block(700, 400, 20, 600));

  scene02.addBlock(new Block(800, 690, 50, 20));
  scene02.addBlock(new Block(800, 590, 50, 20));
  scene02.addBlock(new Block(800, 490, 50, 20));
  scene02.addBlock(new Block(800, 390, 50, 20));
  scene02.addBlock(new Block(800, 290, 50, 20));
  scene02.addBlock(new Block(800, 190, 50, 20));

  scene02.addDoor(new Door(800, 140, 50, 50, scene03));

  for (int i = 0; i < 24; i++) {
    mn02 = new MoveNeedle(220 + i * 20, 90, 20, 20, 220 + i * 20, 1000, 60);
    mn02.setButton(865, 55, 50, 50, scene02);
    scene02.addMoveNeedle(mn02);
  }

  for (int i = 0; i < 24; i++) {
    mn02 = new MoveNeedle(220 + i * 20, 910, 20, 20, 220 + i * 20, 10, 60);
    mn02.setTrap(450, 125, 480, 50, scene02);
    scene02.addMoveNeedle(mn02);
  }

  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(900, 650 - i * 20, 20, 20, 800, 650 - i * 20, 20);
    mn02.setTrap(742.5, 690, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(700, 550 - i * 20, 20, 20, 800, 550 - i * 20, 20);
    mn02.setTrap(858, 590, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(900, 450 - i * 20, 20, 20, 800, 450 - i * 20, 20);
    mn02.setTrap(742.5, 490, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(700, 350 - i * 20, 20, 20, 800, 350 - i * 20, 20);
    mn02.setTrap(858, 390, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(900, 250 - i * 20, 20, 20, 800, 250 - i * 20, 20);
    mn02.setTrap(742.5, 290, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = -1; i < 3; i++) {
    mn02 = new MoveNeedle(700, 150 - i * 20, 20, 20, 800, 150 - i * 20, 20);
    mn02.setTrap(742.5, 190, 65, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }

  for (int i = 0; i < 12; i++) {
    mn02 = new MoveNeedle(1080, 790 - i * 59, 20, 20, 920, 790 - i * 59, 15);
    mn02.setTrap(1000, 790 - i * 59, 180, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }
  for (int i = 0; i < 12; i++) {
    mn02 = new MoveNeedle(920, 790 - i * 59, 20, 20, 1080, 790 - i * 59, 15);
    mn02.setTrap(1000, 790 - i * 59, 180, 20, scene02);
    scene02.addMoveNeedle(mn02);
  }

  for (int i = 0; i < 8; i++) {
    mn02 = new MoveNeedle(30 + i * 22, 890, 20, 20, 30 + i * 21, 135, 240);
    mn02.setTrap(105, 660, 170, 40, scene02);
    scene02.addMoveNeedle(mn02);
  }

  for (int i = 0; i < 15; i++) {
    if (i < 13) {
      mn02 = new MoveNeedle(900 - i * 50, 810, 20, 20, 900 - i * 50, 1000, 60);
    } else {
      mn02 = new MoveNeedle(900 - i * 50, 810, 20, 20, 900 - i * 50, 1000, 20);
    }
    mn02.setTrap(900 - i * 50, 840, 30, 60, scene02);
    scene02.addMoveNeedle(mn02);
  }

  //scene02.addBoss(new Boss(600, 750, 100, 200));

  //scene02.addNeedle(new Needle(255, 860, 20, 20));

  scene03.addBlock(new Block(width/2, height-10, width, 20));
  scene03.addBlock(new Block(10, height/2, 20, height));
  scene03.addBlock(new Block(width - 10, height/2, 20, height));

  scene03.addBlock(new Block(100, 700, 5, 5));
  scene03.addBlock(new Block(200, 100, 20, 20));
  scene03.addBlock(new Block(300, 500, 20, 20));

  scene03.addBlock(new Block(800, 190, 50, 20));
  scene03.addDoor(new Door(800, 140, 50, 50, scene02));


  playerPosX = 100;
  playerPosY = 860;
  nowScene = scene03;

  spawnPosX = playerPosX;
  spawnPosY = playerPosY;
  spawnScene = nowScene;


  for (int i = 0; i < 360; i++) {
    println(i  + " : " + sin(i/180.0 * PI));
  }
}

void draw() {
  background(255);

  int curTime = millis();
  elapsedTime += curTime - lastUpdateTime;
  lastUpdateTime = curTime;

  nowScene.update();

  PlayerMove();
  PlayerDisplay();

  displayDebug();
}

void PlayerMove() {
  if (cannotMove == false) {
    if (jumpMove == true)
    {
      if (isJumping == false) {
        jumpPower = 12.0;
        isJumping = true;
      }
    }

    jumpPower -= gravity * 0.05;

    if (jumpMove == false && jumpPower > 0) {
      jumpPower *= 0.45;
    }

    playerPosY += -jumpPower;

    for (Block b : nowScene.blocks) {
      if (abs(playerPosX - b.posX) < playerScaleX/2 + b.scaleX/2
        && abs(playerPosY - b.posY) < playerScaleY/2 + b.scaleY/2) {
        if (jumpPower >= 0) {
          // 頭がぶつかったとき
          playerPosY = b.posY + b.scaleY/2 + playerScaleY/2;
          jumpPower = 0;
        } else {
          // 地面に着地した時
          playerPosY = b.posY - b.scaleY/2 - playerScaleY/2;
          jumpPower = 0;
          isJumping = false;
          isSecondJumping = false;
          secondJumpMove = false;
        }
      }
    }

    // 二段ジャンプ
    if (isJumping == true && jumpPower <= 0) {
      secondJumpMove = true;
    }
    if (jumpMove == true && isJumping == true 
      && secondJumpMove == true && isSecondJumping == false) {
      jumpPower = 10.0;
      isSecondJumping = true;
    }

    if (jumpPower < 0) {
      isJumping = true;
    }

    if (rightMove == true && leftMove == false) {
      playerPosX += speed;
      for (Block b : nowScene.blocks) {
        if (abs(playerPosX - b.posX) < playerScaleX/2 + b.scaleX/2
          && abs(playerPosY - b.posY) < playerScaleY/2 + b.scaleY/2) {
          playerPosX = b.posX - b.scaleX/2 - playerScaleX/2;
        }
      }
      isRighting = true;
    }
    if (leftMove == true && rightMove == false) {
      playerPosX -= speed;
      for (Block b : nowScene.blocks) {
        if (abs(playerPosX - b.posX) < playerScaleX/2 + b.scaleX/2
          && abs(playerPosY - b.posY) < playerScaleY/2 + b.scaleY/2) {
          playerPosX = b.posX + b.scaleX/2 + playerScaleX/2;
        }
      }
      isRighting = false;
    }

    // しゃがみ処理
    if (downMove == true) {
      speed = 1.0;
      attackSpeed = defaultAttackSpeed;
      if (playerScaleY != playerScaleY_down) {
        playerPosY += playerScaleY_down/2.0;
      }
      playerScaleY = playerScaleY_down;
    } else {
      speed = 5.0;
      attackSpeed = defaultAttackSpeed/2.0;
      if (playerScaleY != playerScaleY_default) {
        playerPosY -= playerScaleY_down/2.0;
      }
      playerScaleY = playerScaleY_default;
    }
  }

  // 攻撃処理
  attackCount--;

  if (cannotMove == false) {
    if (attackMove == true && attackCount <= 0) {
      if (isRighting) {
        nowScene.addShot(new Shot(playerPosX, playerPosY, 10, playerScaleY/2.0, attackSpeed, 1));
      } else {
        nowScene.addShot(new Shot(playerPosX, playerPosY, 10, playerScaleY/2.0, attackSpeed, -1));
      }

      attackCount = attackCountSpan;
    }
  }
}

void PlayerDisplay() {
  stroke(0);
  strokeWeight(1);
  fill(46, 139, 87);
  rect(playerPosX, playerPosY, playerScaleX, playerScaleY);

  if (isRighting == true) {
    fill(200, 10, 10);
    rect(playerPosX + playerScaleX/2.0, playerPosY, playerScaleX/2.0, playerScaleY/4.0);
  } else {
    fill(200, 10, 10);
    rect(playerPosX - playerScaleX/2.0, playerPosY, playerScaleX/2.0, playerScaleY/4.0);
  }
}

void flagReset() {
  playerPosX = spawnPosX;
  playerPosY = spawnPosY;
  nowScene = spawnScene;

  jumpPower = 0;
  isSecondJumping = false;

  Scene[] scenes = new Scene[]{scene01, scene02, scene03};

  for (Scene s : scenes) {
    for (MoveNeedle mn : s.moveNeedles) {
      mn.lerpCount = 0;
      mn.move();
    }
    for (ButtonBlock bb : s.buttonBlocks) {
      bb.action = false;
    }
  }
}

void displayDebug() {
  if (debugMode == true) {
    fill(0);
    textSize(30);
    textAlign(LEFT, TOP);
    text("FPS : " + round(frameRate), 70 * centiWidth, 5 * centiHeight);
    text("P   : {" + round(playerPosX) + ", "+ round(playerPosY) + ", " + 0 + "}", 70 * centiWidth, 10 * centiHeight);
  }
}

void keyPressed() {
  if (keyCode == UP || keyCode == 87) {
    upMove = true;
  }
  if (keyCode == DOWN || keyCode == 83) {
    downMove = true;
  }
  if (keyCode == LEFT || keyCode == 65) {
    leftMove = true;
  }
  if (keyCode == RIGHT || keyCode == 68) {
    rightMove = true;
  }
  if (keyCode == SHIFT) {
    jumpMove = true;
  }
  if (keyCode == CONTROL) {
    attackMove = true;
  }

  if (keyCode == 114) {
    debugMode = !debugMode;
  }

  if (key == 'r' || key == 'R') {
    flagReset();
  }
}

void keyReleased() {
  if (keyCode == UP || keyCode == 87) {
    upMove = false;
  }
  if (keyCode == DOWN || keyCode == 83) {
    downMove = false;
  }
  if (keyCode == LEFT || keyCode == 65) {
    leftMove = false;
  }
  if (keyCode == RIGHT || keyCode == 68) {
    rightMove = false;
  }
  if (keyCode == SHIFT) {
    jumpMove = false;
  }
  if (keyCode == CONTROL) {
    attackMove = false;
    attackCount = 0;
  }
}

void mousePressed() {
  println("mouse = (" + mouseX + ", " + mouseY+ ")");
  playerPosX = mouseX;
  playerPosY = mouseY;
}
