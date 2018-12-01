//タイトルシーン用変数
int pcs_a;
boolean pcs;

//シーンのstart用変数
boolean selectSet;
boolean scene01;
boolean scene02;
boolean scene03;
boolean scene04;
boolean scene05;

void title() {
  fill(200, 150);
  rect(300, height/2, 600, height);

  if (pcs) {
    pcs_a-=5;
  } else {
    pcs_a+=5;
  }
  if (pcs_a > 255) {
    pcs = true;
  } else if (pcs_a < 0) {
    pcs = false;
  }
  fill(255, pcs_a);
  text("©SKILLFUL0515", 20*600/100, 95*height/100);
  select[101].update();
  select[102].update();
}

void selectScene() {
  if (selectSet) {
    DeleteAndSet();
    selectSet = false;
  }
  for (int i = 0; i < 10; i++) {
    select[i].update();
  }
  powerUp[0].update();
  powerUp[1].update();
}

void scene01() {
  if (scene01) {
    //bosses.add(new Boss01(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 50, 2, 1000, 10, 60.0, true));
    bosses.add(new Boss01(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 50, 1, 1000, 100, 60.0, true));
    Boss_HpBarSet();
    scene01=false;
  }

  update();
  delete();
  panel();    
  hp_Bar();
  Boss_HpBar();

  //雑魚を一定間隔で出現
  if (frameCount%120 == 0) {
    int r = int(random(100));
    if (r == 15) {
      healings.add(new Healing(random(10, 600-10), 0, random(0.1, 0.5)));
    } else {
      //Enemy(float argX, float argY, float argWidth, float argHeight, color argColor, float argSpeed, int argHp, int argDefense, int argHaveScore, int argHaveCoin)
      zako01.add(new Zako01(random(10, 590), 0, 40, 20, color(255, 0, 0), random(0.1, 1.0), 1, 1, 50, 1, false));
    }
  }

  //ゲームオーバー判定
  gameOverDecision();

  //クリア処理
  if (bosses.size() == 0) {
    player.guard = true;
    select[100].update();
    select[0].clear = true;
  }
}

void scene02() {
  if (scene02) {
    //bosses.add(new Boss01(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 50, 2, 1000, 10, 60.0, true));
    bosses.add(new Boss02(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 150, 1, 1000, 100, 60.0, true));
    Boss_HpBarSet();
    scene02=false;
  }

  update();
  delete();
  panel();    
  hp_Bar();
  Boss_HpBar();

  //雑魚を一定間隔で出現
  if (frameCount%120 == 0) {
    int r = int(random(100));
    if (r == 15) {
      healings.add(new Healing(random(10, 600-10), 0, random(0.1, 0.5)));
    } else {
      //Enemy(float argX, float argY, float argWidth, float argHeight, color argColor, float argSpeed, int argHp, int argDefense, int argHaveScore, int argHaveCoin)
      zako01.add(new Zako01(random(10, 590), 0, 40, 20, color(255, 0, 0), random(0.1, 1.0), 1, 1, 50, 1, false));
    }
  }

  //ゲームオーバー判定
  gameOverDecision();

  //クリア処理
  if (bosses.size() == 0) {
    player.guard = true;
    select[100].update();
    select[1].clear = true;
  }
}

void scene03() {
  if (scene03) {
    //bosses.add(new Boss01(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 50, 2, 1000, 10, 60.0, true));
    bosses.add(new Boss03(600/2, 15*height/100, 100, 50, color(255, 0, 255), 1.5, 50, 1, 1000, 100, 60.0, true));
    Boss_HpBarSet();
    scene03=false;
  }

  update();
  delete();
  panel();    
  hp_Bar();
  Boss_HpBar();

  //雑魚を一定間隔で出現
  if (frameCount%120 == 0) {
    int r = int(random(100));
    if (r == 15) {
      healings.add(new Healing(random(10, 600-10), 0, random(0.1, 0.5)));
    } else {
      //Enemy(float argX, float argY, float argWidth, float argHeight, color argColor, float argSpeed, int argHp, int argDefense, int argHaveScore, int argHaveCoin)
      zako01.add(new Zako01(random(10, 590), 0, 40, 20, color(255, 0, 0), random(0.1, 1.0), 1, 1, 50, 1, false));
    }
  }

  //ゲームオーバー判定
  gameOverDecision();

  //クリア処理
  if (bosses.size() == 0) {
    player.guard = true;
    select[100].update();
    select[2].clear = true;
  }
}


void scene04() {
  if (scene04) {
    /*
    (float argX, float argY, float argWidth, float argHeight, 
     color argColor, float argSpeed, int argHp, int argDefense, 
     int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode)
     */
    bosses.add(new Boss01(600/2, 15*height/100, 100, 50, color(255, 0, 255), 3.0, 100, 1, 1000, 100, 60.0, true));
    Boss_HpBarSet();
    scene04=false;
  }

  update();
  delete();
  panel();    
  hp_Bar();
  Boss_HpBar();

  //雑魚を一定間隔で出現
  if (frameCount%60 == 0) {
    int r = int(random(100));
    if (r == 15) {
      healings.add(new Healing(random(10, 600-10), 0, random(0.1, 0.5)));
    } else {
      //Enemy(float argX, float argY, float argWidth, float argHeight, color argColor, float argSpeed, int argHp, int argDefense, int argHaveScore, int argHaveCoin)
      zako01.add(new Zako01(random(10, 590), 0, 40, 20, color(255, 0, 0), random(0.1, 1.0), 1, 1, 50, 1, false));
    }
  }

  //ゲームオーバー判定
  gameOverDecision();

  //クリア処理
  if (bosses.size() == 0) {
    player.guard = true;
    select[100].update();
    select[3].clear = true;
  }
}

void scene05() {
  if (scene05) {
    /*
    (float argX, float argY, float argWidth, float argHeight, 
     color argColor, float argSpeed, int argHp, int argDefense, 
     int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode)
     */
    bosses.add(new Boss02(600/2, 15*height/100, 100, 50, color(255, 0, 255), 3.0, 300, 1, 1000, 100, 60.0, true));
    Boss_HpBarSet();
    scene05=false;
  }

  update();
  delete();
  panel();    
  hp_Bar();
  Boss_HpBar();

  //雑魚を一定間隔で出現
  if (frameCount%30 == 0) {
    int r = int(random(100));
    if (r == 15) {
      healings.add(new Healing(random(10, 600-10), 0, random(0.1, 0.5)));
    } else {
      //Enemy(float argX, float argY, float argWidth, float argHeight, color argColor, float argSpeed, int argHp, int argDefense, int argHaveScore, int argHaveCoin)
      zako01.add(new Zako01(random(10, 590), 0, 40, 20, color(255, 0, 0), random(0.1, 1.0), 1, 1, 50, 1, false));
    }
  }

  //ゲームオーバー判定
  gameOverDecision();

  //クリア処理
  if (bosses.size() == 0) {
    player.guard = true;
    select[100].update();
    select[4].clear = true;
  }
}

void gameOverDecision() {
  if (player.hp == 0) {
    gameOver();
  } else if (player.muteki > player.muteki_s*45) {
    fill(255, 0, 0, 30);
    rect(width/2, height/2, width, height);
  }
}