class Dead {
  float posx;
  float posy;
  float angle;
  float speed;
  float angleSpeed;
  boolean del;
  float delCount;

  Dead(float argX, float argY, float argAngle, float argSpeed, float argAngleSpeed, float argDelCount) {
    posx = argX;
    posy = argY;
    angle = argAngle;
    speed = argSpeed;
    angleSpeed = argAngleSpeed;
    del = false;
    delCount = argDelCount;
  }

  void update() {
    display();
    move();
    delCount -= 1.0/60.0;
    if (delCount <= 0) {
      del = true;
    }
  }

  void display() {
    fill(255, 165, 0);
    ellipse(posx, posy, 6, 6);
  }

  void move() {
    angle = (angle + angleSpeed) % 360;
    posx += cos(radians(angle)) * speed;
    posy += sin(radians(angle)) * speed;
  }
}

class Explosion {
  float posx;
  float posy;
  float wid;
  float hei;
  float lag;
  boolean heal;
  float count;
  boolean del;
  PImage[] bombs = new PImage[6];

  Explosion(float argX, float argY, float argWidth, float argHeight, float argLag, boolean argHealMode) {
    posx = argX;
    posy = argY;
    wid = argWidth;
    hei = argHeight;
    lag = argLag;
    heal = argHealMode;
    count = 0;
    del = false;
    for (int i = 0; i < 6; i++) {
      bombs[i] = loadImage("images/bombing/0"+ str(i+1) + ".png");
    }
  }

  void update() {
    lag -= 1.0/60.0;
    if (lag <= 0) {
      display();
      count += 0.2;
      if (count >= 6.0) {
        del = true;
      }
    }
  }

  void display() {
    if (heal) {
      tint(61, 255, 61);
      image(bombs[int(count)], posx, posy, wid, hei);
      noTint();
    } else {
      image(bombs[int(count)], posx, posy, wid, hei);
    }
  }
}

class Coin {
  float posx;
  float posy;
  PVector v;
  float vx;
  float vy;
  float speed;

  Coin(float argX, float argY) {
    posx = argX;
    posy = argY;
    speed = random(7.0, 9.0);
  }

  void setVel() {
    vx = 630 - posx;
    vy = 15*height/32 - posy;
    v = new PVector(vx, vy);
    v.normalize();
    vx = v.x;
    vy = v.y;
  }

  void update() {
    setVel();
    display();
    move();
  }

  void display() {
    fill(255, 215, 0);
    ellipse(posx, posy, 10, 10);
  }

  void move() {
    posx += vx*speed;
    posy += vy*speed;
  }

  boolean hit() {
    if (dist(posx, posy, 630, 15*height/32) <= 10) {
      money++;
      return true;
    } else {
      return false;
    }
  }
}

class DamageText {
  float posx;
  float posy;
  String text;
  float speedx;
  float speedy;
  int alpha;
  boolean del;

  DamageText(float argX, float argY, String argText) {
    posx = argX;
    posy = argY;
    text = argText;

    speedx = random(-2, 2);
    speedy = random(-5, -2);
    alpha = 255;
    del = false;
  }

  void update() {
    display();
    move();
    alpha-=3;
    if (alpha <= 0) {
      del = true;
    }
  }

  void display() {
    fill(255, alpha);
    textSize(18+(int(text)/5)*3);
    text(text, posx, posy);
  }

  void move() {
    posx += speedx;
    speedy += 5.0/frameRate;
    posy += speedy;
  }
}