import { Component, inject, OnInit, signal } from '@angular/core';
import { Photo } from '../../../types/member';
import { AdminService } from '../../../core/services/admin-service';

@Component({
  imports: [],
  selector: 'app-photo-management',
  styleUrl: './photo-management.css',
  templateUrl: './photo-management.html',
})
export class PhotoManagement implements OnInit {
  photos = signal<Photo[]>([]);
  private adminService = inject(AdminService);

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos.set(photos)
    })
  }
  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => this.photos.update(photos => {
        return photos.filter(x => x.id !== photoId)
      })
    })
  }
  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => this.photos.update(photos => {
        return photos.filter(x => x.id !== photoId)
      })
    })
  }
}
