require 'rubygems'

desc 'Publish IntAirActImageWindows'
task :publish => ['demo:publish']

def publish(name, folder)
  cd name do
    rm_rf 'ase-lab.github.com'
    system('git clone https://github.com/ase-lab/ase-lab.github.com.git')
    cd 'ase-lab.github.com' do
      system('git rm -r ' + folder)
      mv('../publish', folder)
      system('git add ' + folder)
      system('git commit -m "Updated ' + name + '"')
      system('git push')
    end
    rm_rf 'ase-lab.github.com'
  end
end

namespace :demo do

  desc 'Publish IntAirActImageWindows'
  task :publish do
    publish('IntAirActImageWindows', 'intairact-image-windows')
  end

end

desc 'Initialize and update all submodules recursively'
task :init do
  system('git submodule update --init --recursive')
  system('git submodule foreach --recursive "git checkout master"')
end

desc 'Pull all submodules recursively'
task :pull => :init do
  system('git submodule foreach --recursive git pull')
end
