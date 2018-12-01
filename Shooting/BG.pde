class BGStar {
  float posx;
  float posy;
  float rad;
  float col_a;

  BGStar() {
    posx = random(0.0, 600.0);
    posy = random(-height, 0.0);
    rad = random(0.1, 5.0);
    col_a = random(255);
  }

  void update() {
    display();
    move();
  }

  void display() {
    col_a-=3.0*rad/5.0;
    if(col_a <= 0){
      col_a = 255;
    }
    fill(255, int(col_a));
    ellipse(posx, posy, rad, rad);
  }

  void move() {
    posy += BGSpeed*rad/5.0;
    if (posy > height) {
      posx = random(0.0, 600.0);
      posy = 0.0;
      col_a = int(random(255));
    }
  }
}