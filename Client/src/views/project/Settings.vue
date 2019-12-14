<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* Main Heading -->
      <v-flex xs12>
        <h2
          class="headline mb-3"
          v-t="'project.general'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p
              class="caption mb-0 grey--text"
              v-t="'core.name'"
            />
            <p class="subheading">
              {{ name }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'core.email'"
            />
            <p class="subheading mb-0">
              {{ email }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              :to="{ name: 'updateGeneral' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* News Heading -->
      <v-flex xs12>
        <h2
          class="headline mb-3"
          v-t="'project.news'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="subheading mb-0">
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Animi autem quas, veniam suscipit numquam sunt quo a eum eligendi rerum exercitationem doloremque velit. Veritatis odio, ratione commodi impedit earum cumque.
              <br>
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis, et, fuga eveniet nesciunt atque magnam, sunt non natus alias nobis culpa. Natus repudiandae, necessitatibus nam doloribus excepturi voluptatibus consectetur ad?
            </p>
          </v-card-text>
        </v-card>
      </v-flex>

      <!--//* Knowledge Base Heading -->
      <v-flex xs12>
        <h2
          class="headline mb-3"
          v-t="'knowledgeBase.topics'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <p>
          {{ $t('knowledgeBase.topicsDescription') }}
        </p>
        <v-card flat>
          <v-list>
            <v-list-tile
              v-for="(topic, index) in topics"
              :key="topic.name"
            >
              <v-list-tile-content>
                <v-list-tile-title v-text="topic.name" />
              </v-list-tile-content>

              <v-list-tile-action style="flex-direction:row; align-items:center;">
                <v-btn icon class="ma-0">
                  <v-icon color="grey">edit</v-icon>
                </v-btn>
                <v-btn icon class="ma-0">
                  <v-icon color="grey">delete</v-icon>
                </v-btn>
                <v-btn
                  v-if="index > 0"
                  icon
                  class="ma-0"
                >
                  <v-icon color="grey">expand_less</v-icon>
                </v-btn>
                <v-btn
                  v-if="index < topics.length - 1"
                  icon
                  class="ma-0"
                >
                  <v-icon color="grey">expand_more</v-icon>
                </v-btn>
              </v-list-tile-action>
            </v-list-tile>
          </v-list>
        </v-card>
        <v-flex
          xs12
          class="mt-2"
        >
          <v-btn
            :to="{ name: 'create' }"
            color="primary"
            v-t="'knowledgeBase.createTopic'"
          />
        </v-flex>
      </v-flex>

      <!--//* Critical Heading -->
      <v-flex xs12>
        <h2
          class="headline mb-3 error--text"
          v-t="'project.criticalArea'"
        />
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card
          flat
          dark
          color="red lighten-4"
        >
          <v-card-text>
            <DeleteProjectDialog />
          </v-card-text>
        </v-card>
      </v-flex>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import DeleteProjectDialog from '@/components/dialogs/DeleteProjectDialog.vue'
import Component from 'vue-class-component'
import ProjectModule from '@/store/modules/projects'
import { getModule } from 'vuex-module-decorators'
import { Project, Topic } from '@/models/definitions'
import { Watch } from 'vue-property-decorator'
import KnowledgeBaseModule from '../../store/modules/knowledgeBase'

@Component({
  components: { DeleteProjectDialog }
})
export default class Settings extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private name: string = ''
  private email: string = ''
  private topics: Topic[] = []

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.loadProject()
  }

  private async created() {
    await this.loadProject()
  }

  private async loadProject() {
    const projectId = this.$route.params.projectId
    const project = this.projectModule.myProjects.filter((x: Project) => x.id === projectId)[0]
    if (project) {
      this.name = project.name
      this.email = project.email

      this.topics = this.knowledgeBaseModule.allTopics.filter((x: Topic) => x.projectId === projectId)
    }
  }
}
</script>
