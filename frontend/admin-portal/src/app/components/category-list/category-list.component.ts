import { Subject, takeUntil } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CategoryApiService } from 'src/app/core/services/category-api.service';
import { categoryRequest, categoryResponse } from 'src/app/core/types/category';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
})
export class CategoryListComponent implements OnInit, OnDestroy {
  categoryList: categoryResponse[] = [];

  // Anology : destroy is doorbell button
  destroy$ = new Subject<void>();
  constructor(private catservice: CategoryApiService) {}

  ngOnInit() {
    this.getAllCategories();
  }

  getAllCategories() {
    this.catservice.getAll()
      .pipe(
        // if it hears the next() notification, it will immediately turn off the Observable (API Call)
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res) => {
          if (res) {
            this.categoryList = res;
          } else {
            this.categoryList = [];
          }
        },
        error: (err) => {
        alert(err?.error?.message ?? 'something went wrong');
      },
      });
  }

  removeCategory(id: string) {
    if(!confirm("Are you sure you want to remove?")){
      return;
    }

    this.catservice.remove(id).subscribe({
      next: () => {
        alert('Category Successfully Removed!');
        this.getAllCategories();
      },
      error: (err) => {
        alert(err?.error?.message ?? 'something went wrong');
      },
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
