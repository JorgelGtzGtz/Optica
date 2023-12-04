import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.css']
})
export class TabsComponent implements OnInit {
  @Input() tabs: string[] = [];
  @Output() selectedTab = new EventEmitter<string>();
  sselectedTab: string;
  
  constructor() { }

  ngOnInit() {
  }

  selectTab(tab: string) {
    this.sselectedTab = tab;
    this.selectedTab.emit(tab);
  }

}
