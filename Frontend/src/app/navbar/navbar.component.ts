import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from '../services/login-service/login-service.service';
import { SessionStorageService } from '../services/session-storage/session-storage.service';
import { EstablishmentService } from '../services/API-implementations/establishment.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  constructor(
    private router: Router,
    private login: LoginService,
    private sessionStorage: SessionStorageService,
    private establishmentService: EstablishmentService
  ) {}

  protected activeEstablishment?: string;

  ngOnInit(): void {
    this.getLoggedInEstablishmentName();
  }

  async getLoggedInEstablishmentName() {
    var establishmentid = this.sessionStorage.getActiveEstablishment();
    if (establishmentid) {
      var name = await this.establishmentService.getEstablishmentsDTO([
        establishmentid,
      ]);
      this.activeEstablishment = name[0].name;
    }
  }

  onClustering() {
    this.router.navigate(['/clustering']);
  }

  onCorrelation() {
    this.router.navigate(['/correlation']);
  }

  onSelectEstablishment() {
    this.router.navigate(['/select-establishment']);
  }

  onLogout() {
    this.login.LogOut();
    this.router.navigate(['/login']);
  }
}
