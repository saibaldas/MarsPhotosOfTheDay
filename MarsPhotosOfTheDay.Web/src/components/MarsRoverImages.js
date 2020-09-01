import React, { Component } from 'react';

export class MarsRoverImages extends Component {
  static displayName = MarsRoverImages.name;

  constructor(props) {
    super(props);
    this.state = { photos: [], loading: true, dateToSearch: '' };
  }

  componentDidMount() {
    //this.populateMarsRoverImagesData();
  }

  handleDownload = (imageSrc) => {
    console.log("download " + imageSrc)
		fetch('https://localhost:5001/MarsPhotosOfTheDay/download?url=' + imageSrc)
			.then(response => {
				response.blob().then(blob => {
					let url = window.URL.createObjectURL(blob);
          let a = document.createElement('a');
          var fileName = imageSrc.substring(imageSrc.lastIndexOf('/')+1);
					a.href = url;
					a.download = fileName;
					a.click();
				});
				//window.location.href = response.url;
		});
  }

  handleChange = (e) => {
    this.setState({ dateToSearch: e.target.value });
  }
  
  renderMarsRoverImagesTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <input
              id="dateToSearch"
              onChange={this.handleChange}
              value={this.state.dateToSearch}
            />
            <button onClick={() => this.populateMarsRoverImagesData()}>Show Images</button>
          </tr>
        </thead>
        <tbody>
          <div>
            <ul>
              {this.state.photos.map(photo =>
                <li key={photo.img_src}>
                      <img src={photo.img_src} width="100" height="100"/>
                      <button onClick={() => this.handleDownload(photo.img_src)}>Download</button>
                </li>
              )}
            </ul>
          </div>
        </tbody>
      </table>
    );
  }

  render() {
    //let contents = this.state.loading
    //  ? <p><em>Loading...</em></p>
    //    : this.renderMarsRoverImagesTable();
    let contents = this.renderMarsRoverImagesTable();
    return (
      <div>
        <h1 id="tabelLabel" >Images</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

    async populateMarsRoverImagesData() {
        const response = await fetch('https://localhost:5001/MarsPhotosOfTheDay/urls?date=' + this.state.dateToSearch);
    const data = await response.json();
    this.setState({ photos: data, loading: false });
  }
}
