import { Component, HostListener } from "@angular/core";

@Component({
  selector: 'ab-footer',
  templateUrl: 'footer.component.html',
  styleUrls: ['footer.component.css']
})
export class FooterComponent {
  isBtnVisible = false;
  private window: Window;
  currentYear: number;
  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    this.window = window;
    this.isBtnVisible = this.window.pageYOffset !== 0;
  }

  constructor() {
    this.currentYear = new Date().getFullYear();
  }

  scrollToTop(behavior: 'auto' | 'smooth' = 'smooth') {
    window.scrollTo({
      top: 0,
      left: 0,
      behavior
    });
  }
}
