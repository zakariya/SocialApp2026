import { Component, ElementRef, Input, input, model, output, ViewChild } from '@angular/core';
import { MemberParams } from '../../../types/member';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule],
  selector: 'app-filter-modal',
  styleUrl: './filter-modal.css',
  templateUrl: './filter-modal.html',
})
export class FilterModal {
  @ViewChild('filterModal') modalRef!: ElementRef<HTMLDialogElement>;
  closeModal = output();
  sumbitData = output<MemberParams>();
  memberParams = model(new MemberParams());
  @Input() params!: MemberParams;
  constructor() {
    const filters = localStorage.getItem('filters');
    if (filters) {
      this.memberParams.set(JSON.parse(filters));
    }
  }

  open() {
    this.modalRef.nativeElement.showModal();
  }
  close() {
    this.modalRef.nativeElement.close();
    this.closeModal.emit();
  }

  sumbit() {
    this.sumbitData.emit(this.memberParams());
    this.close();
  }

  onMinAgeChange() {
    if (this.memberParams().minAge < 18) {
      this.memberParams().minAge = 18;
    }
  }

  onMaxAgeChange() {
    if (this.memberParams().maxAge < this.memberParams().minAge) {
      this.memberParams().maxAge = this.memberParams().minAge;
    }

  }
}
