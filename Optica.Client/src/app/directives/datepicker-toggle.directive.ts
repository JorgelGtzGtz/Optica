import { ComponentRef, Directive, ElementRef, HostListener, Input } from '@angular/core';
import { NgbDatepicker, NgbInputDatepicker } from '@ng-bootstrap/ng-bootstrap';

@Directive({
  selector: '[datepickerToggle]',
})
export class DatepickerToggleDirective {
  // tslint:disable-next-line no-input-rename
  @Input('datepickerToggle') inputDatepicker: NgbInputDatepicker;
  @Input() closeDatepickerOnClick = true;

  constructor(private _elementRef: ElementRef) {}

  @HostListener('click')
  public toggle() {
    if (this.inputDatepicker.isOpen() && this.closeDatepickerOnClick) {
      this.inputDatepicker.close();
    } else {
      this.inputDatepicker.open();
    }
  }

  @HostListener('document:click', ['$event'])
  public closeOnOutsideEvent($event: MouseEvent) {
    if (this.inputDatepicker.isOpen()) {
      if (!this.isTargettingToggleButton($event) && this.shouldCloseOnOutsideEvent($event)) {
        this.inputDatepicker.close();
      }
    }
  }

  private isTargettingToggleButton($event: MouseEvent): boolean {
    return this._elementRef.nativeElement.contains($event.target);
  }

  private shouldCloseOnOutsideEvent($event: MouseEvent): boolean {
    const inputDatepickerElRef: ElementRef = (this.inputDatepicker as any)._elRef;
    const datepickerCmpRef: ComponentRef<NgbDatepicker> = (this.inputDatepicker as any)._cRef;

    if (inputDatepickerElRef != null && datepickerCmpRef != null) {
      let popupClick = false;
      const inputClick = inputDatepickerElRef.nativeElement.contains($event.target);

      if (this.inputDatepicker.isOpen() && datepickerCmpRef.location.nativeElement.contains($event.target)) {
        popupClick = true;
      }
      return !inputClick && !popupClick;
    } else {
      return false;
    }
  }
}