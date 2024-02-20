import { NgModule } from '@angular/core';
import { RouterModule, Routes, UrlSegment } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SelectEstablishmentComponent } from './select-establishment/select-establishment.component';
import { ClusterComponent } from './cluster/cluster.component';
import { CrossCorrelationComponent } from './cross-correlation/cross-correlation.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'select-establishment', component: SelectEstablishmentComponent },
  {
    path: 'clustering',
    component: ClusterComponent,
  },
  {
    path: 'correlation',
    component: CrossCorrelationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
