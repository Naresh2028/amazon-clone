import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { CategoryCreateComponent } from './components/category-create/category-create.component';
import { CategoryUpdateComponent } from './components/category-update/category-update.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

const routes: Routes = [
  {path:'',component:CategoryListComponent},
  {path:'category-create',component:CategoryCreateComponent},
  {path:'category-update/:id',component:CategoryUpdateComponent},
  {path:'**', component:NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
