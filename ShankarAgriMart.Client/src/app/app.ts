import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {FooterComponent} from './layout/components/footer/footer';
import { NavbarComponent } from './layout/components/navbar/navbar';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent
],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  protected readonly title = signal(
    'ShankarAgriMart.Client'
  );

}