class Enemy {
  // 敵の基本情報の設定
  float posx;
  float posy;
  float wid;
  float hei;
  color col;
  float speed;
  int hp;
  int defense;
  int haveScore;
  int haveCoin;
  boolean bossMode;

  boolean del;
  float hitEffect_s;
  float hitEffect;

  Enemy(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, boolean argBossMode) {
    posx = argX;
    posy = argY;
    wid = argWidth;
    hei = argHeight;
    col = argColor;
    speed = argSpeed;
    hp = argHp;
    defense = argDefense;
    haveScore = argHaveScore;
    haveCoin = argHaveCoin;
    bossMode = argBossMode;

    del = false;
    hitEffect_s = 0.15;//単位は秒、何秒表示させるか
    hitEffect = 0.0;
  }

  void update() {
    display();
    hit();
    delete();
    hitEffect--;
  }

  void display() {
    fill(col);
    rect(posx, posy, wid, hei);
    if (hitEffect > 0) {
      fill(255, 200);
      rect(posx, posy, wid, hei);
    }
  }

  void damage(int val) {
    hp -= val;
    damageTexts.add(new DamageText(posx, posy-hei/2.0, str(val)));
    if (hp <= 0) {
      del = true;
      score += haveScore;
      /*
      for (float angle = 0; angle < 360; angle += 45) {
       deads.add(new Dead(posx, posy, angle, 2, 5, 0.5));
       }
       */
      //explosionEffect表示
      if (!bossMode) {
        bombs.add(new Explosion(posx, posy, 30, 30, 0.0, false));
      } else {
        for (int i = 0; i < 30; i++) {
          float rx = random(-50.0, 50.0);
          float ry = random(-50.0, 50.0);
          float rw = random(-10, 10);
          bombs.add(new Explosion(posx+rx, posy+ry, 50+rw, 50+rw, i/30.0, false));
        }
      }

      for (int i = 0; i < haveCoin; i++) {
        float r = random(-(wid+hei)/4.0, (wid+hei)/4.0);
        coins.add(new Coin(posx+r, posy+r));
      }
    }
    hitEffect = hitEffect_s*60.0;
  }

  void hit() {
    if (abs(posx-player.posx) <= wid/2.0 + player.wid/2.0 
      && abs(posy-player.posy) <= hei/2.0 + player.hei/2.0) {
      player.be_damaged();
    }
  }

  void delete() {
    if (posy >= height+hei/2.0) {
      del = true;
    }
  }
}

class Zako01 extends Enemy {

  Zako01(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, boolean argBossMode) {
    super(argX, argY, argWidth, argHeight, argColor, argSpeed, argHp, argDefense, argHaveScore, argHaveCoin, argBossMode);
  }

  void update() {
    super.update();
    move();
  }

  void move() {
    posy+=speed;
  }
}

class Boss extends Enemy {
  float hp_s;
  float shotSpan;
  int count;

  Boss(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode) {
    super(argX, argY, argWidth, argHeight, argColor, argSpeed, argHp, argDefense, argHaveScore, argHaveCoin, argBossMode);
    shotSpan = argShotSpan;
    count = 0;
    hp_s = float(hp);
  }

  void update() {
    count++;
    super.update();
    shot();
  }

  void shot() {
    //敵のショット処理
    if (count%shotSpan == 0) {
      shotsHoming_ene.add(new ShotHoming_ene(posx, posy, 2, 20, 20, false));
    }
  }
}

class Boss01 extends Boss {

  Boss01(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode) {
    super(argX, argY, argWidth, argHeight, argColor, argSpeed, argHp, argDefense, argHaveScore, argHaveCoin, argShotSpan, argBossMode);
  }

  void update() {
    super.update();
    move();
  }

  void move() {
    posx += speed;
    if (posx >= 600-wid/2 || posx <= wid/2) {
      speed *= -1;
    }
  }
}
class Boss02 extends Boss {

  Boss02(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode) {
    super(argX, argY, argWidth, argHeight, argColor, argSpeed, argHp, argDefense, argHaveScore, argHaveCoin, argShotSpan, argBossMode);
  }

  void update() {
    super.update();
    move();
  }

  void move() {
    posy += speed;
    if (posy >= height-hei/2 || posy <= hei/2) {
      speed *= -1;
    }
  }
}

class Boss03 extends Boss {

  Boss03(float argX, float argY, float argWidth, float argHeight, 
    color argColor, float argSpeed, int argHp, int argDefense, 
    int argHaveScore, int argHaveCoin, float argShotSpan, boolean argBossMode) {
    super(argX, argY, argWidth, argHeight, argColor, argSpeed, argHp, argDefense, argHaveScore, argHaveCoin, argShotSpan, argBossMode);
  }

  void update() {
    super.update();
    move();
  }

  void move() {
    for (Shot s : shots) {
      if (dist(posx, posy, s.posx, s.posy) <= wid/2+20) {
        if (posx >= s.posx) {
          posx += 5.0;
        } else {
          posx -= 5.0;
        }
      }
    }
    posx += speed;
    if (posx >= 600+wid/2) {
      posx = -wid/2;
    } else if (posx <= -wid/2) {
      posx = 600+wid/2;
    }
  }
}

class Healing {
  float posx;
  float posy;
  float speed;
  int hp;
  boolean del;

  Healing(float argX, float argY, float argSpeed) {
    posx = argX;
    posy = argY;
    speed = argSpeed;
    hp = 10;
    del = false;
  }

  void update() {
    display();
    move();
    hit();
    delete();
  }

  void display() {
    fill(0, 255, 0);
    rect(posx, posy, 600/16, height/32);
  }

  void move() {
    posy += speed;
  }

  void damage() {
    hp--;
    if (hp <= 0) {
      del = true;
      score += 10000;
      if (player.hp < 4) {
        player.hp++;
      }
      //explosionEffect表示
      for (int i = 0; i < 5; i++) {
        float rx = random(-15.0, 15.0);
        float ry = random(-15.0, 15.0);
        float rw = random(-10, 10);
        bombs.add(new Explosion(posx+rx, posy+ry, 30+rw, 30+rw, i/30.0, true));
      }
      for (int i = 0; i < 10; i++) {
        float r = random(-2.0, 2.0);
        coins.add(new Coin(posx+r, posy+r));
      }
    }
  }

  void hit() {
    if (dist(0, posy, 0, player.posy) <= height/64+height/32 
      && dist(posx, 0, player.posx, 0) <= 600/32+600/64) {
      player.be_damaged();
    }
  }

  void delete() {
    if (posy >= height) {
      del = true;
    }
  }
}