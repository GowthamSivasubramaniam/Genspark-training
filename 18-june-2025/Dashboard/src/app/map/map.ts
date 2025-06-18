import { Component, Input, OnChanges, SimpleChanges, OnInit } from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-map',
  templateUrl: './map.html',
  styleUrls: ['./map.css']
})
export class MapComponent implements OnInit, OnChanges {
  @Input() stateCounts: { [state: string]: number } = {};

  private map!: L.Map;
  private geojsonLayer!: L.GeoJSON;

  ngOnInit(): void {
    this.initMap();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['stateCounts'] && this.geojsonLayer) {
      this.updateChoroplethStyles();
    }
  }

  private initMap(): void {
    this.map = L.map('map', {
      zoomControl: false,
      scrollWheelZoom: false,
      dragging: true,
      doubleClickZoom: false,
      boxZoom: false,
      keyboard: false,
      maxBounds: [
        [24.396308, -125.0],
        [49.384358, -66.93457]
      ],
      maxBoundsViscosity: 1.0,
      attributionControl: false
    }).setView([37.8, -96], 4);

    fetch('https://rstudio.github.io/leaflet/json/us-states.geojson')
      .then(res => res.json())
      .then(data => {
        this.createChoropleth(data);
       
      });
  }

  private getColor(count: number): string {
    return count === 0 ? 'grey' :   
         count === 1 ? '#81d4fa' :    
         count === 2 ? '#4fc3f7' :   
         count === 3 ? '#29b6f6' :   
         count === 4 ? '#26a69a' :    
         count === 5 ? '#009688' :    
         count <= 10 ? '#FED976' :'white';
  }

  private style(feature: any): L.PathOptions {
    const stateName = feature.properties.name;
    const count = this.stateCounts[stateName] || 0;

    return {
      fillColor: this.getColor(count),
      weight: 2,
      opacity: 1,
      color: 'white',
      dashArray: '3',
      fillOpacity: 0.7
    };
  }

  private createChoropleth(geoData: any): void {
    if (this.geojsonLayer) {
      this.geojsonLayer.remove();
    }

    this.geojsonLayer = L.geoJSON(geoData, {
      style: this.style.bind(this),
      onEachFeature: (feature, layer) => {
        const stateName = feature.properties.name;
        const count = this.stateCounts[stateName] || 0;

        layer.bindTooltip(
          `<strong>${stateName}</strong><br/>Users: ${count}`,
          { direction: 'auto' }
        );

        layer.on({
          mouseover: (e) => {
            const layer = e.target;
            layer.setStyle({
              weight: 5,
              color: '#666',
              dashArray: '',
              fillOpacity: 0.7
            });
            layer.bringToFront();
          },
          mouseout: (e) => {
            this.geojsonLayer.resetStyle(e.target);
          }
        });
      }
    }).addTo(this.map);
  }

  private updateChoroplethStyles(): void {
    this.geojsonLayer.eachLayer((layer: any) => {
      if (layer.feature) {
        const stateName = layer.feature.properties.name;
        const count = this.stateCounts[stateName] || 0;
        layer.setStyle({ fillColor: this.getColor(count) });
        layer.setTooltipContent(`<strong>${stateName}</strong><br/>Users: ${count}`);
      }
    });
  }

  private addLegend(): void {
    const legend = new L.Control({ position: 'bottomright' });

    legend.onAdd = () => {
      const div = L.DomUtil.create('div', 'info legend');
      const grades = [0, 10, 20, 50, 100, 200, 500, 1000];

      for (let i = 0; i < grades.length; i++) {
        div.innerHTML +=
          `<i style="background:${this.getColor(grades[i] + 1)}"></i> ` +
          `${grades[i]}${grades[i + 1] ? '&ndash;' + grades[i + 1] + '<br>' : '+'}`;
      }
      return div;
    };

    legend.addTo(this.map);
  }
}
