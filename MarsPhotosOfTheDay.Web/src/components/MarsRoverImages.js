import React, { Component } from 'react';

export class MarsRoverImages extends Component {
  static displayName = MarsRoverImages.name;

  constructor(props) {
    super(props);
    this.state = { photos: [], loading: true };
  }

  componentDidMount() {
    this.populateMarsRoverImagesData();
  }

  handleClick = (imageSrc) => {
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
  
  renderMarsRoverImagesTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Images</th>
          </tr>
        </thead>
        <tbody>
          <div>
            <ul>
              {this.state.photos.map(photo =>
                <li key={photo.img_src}>
                      <img src={photo.img_src} width="100" height="100"/>
                      <button onClick={() => this.handleClick(photo.img_src)}>Download</button>
                </li>
              )}
            </ul>
          </div>
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : this.renderMarsRoverImagesTable();

    return (
      <div>
        <h1 id="tabelLabel" >Images</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

    async populateMarsRoverImagesData() {
        const response = await fetch('https://localhost:5001/MarsPhotosOfTheDay/urls?date=02%2F27%2F17');
    const data = await response.json();
    this.setState({ photos: data, loading: false });
  }
}
